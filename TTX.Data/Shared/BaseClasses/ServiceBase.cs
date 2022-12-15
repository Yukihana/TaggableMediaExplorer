using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Shared.BaseClasses;

public abstract class ServiceBase
{
    // Configuration

    // Queue

    private readonly ReaderWriterLockSlim _lock = new();
    private readonly SemaphoreSlim _semaphore = new(1);

    // Task

    private readonly List<Task> _tasks = new();

    // Queueing

    public bool TryProcessMessage(IMessage message)
    {
        var valid = MessageTypes.Contains(message.GetType());

        if (valid)
        {
            // Purge expired tasks.
            CleanQueue();

            // Decoupling point: Task has been queued. Source CTS should no longer be able to cancel a queued task. Only the service's own CTS can.
            try
            {
                _lock.EnterWriteLock();
                var task = TaskWrapper(message, _cts.Token);
                task.Start();
                _tasks.Add(task);
            }
            finally { _lock.ExitWriteLock(); }
        }

        return valid;
    }

    private void CleanQueue()
    {
        try
        {
            _lock.EnterUpgradeableReadLock();
            List<Task> expired = new();
            foreach (Task t in _tasks)
            {
                if (t.IsCompleted)
                    expired.Add(t);
            }
            if (expired.Count > 0)
            {
                try
                {
                    _lock.EnterWriteLock();
                    _tasks.RemoveAll(x => expired.Contains(x));
                }
                finally { _lock.ExitWriteLock(); }
            }
        }
        finally { _lock.ExitUpgradeableReadLock(); }
    }

    // Active Task and Shutdown

    private readonly CancellationTokenSource _cts = new();

    private async Task TaskWrapper(IMessage message, CancellationToken token = default)
    {
        try
        {
            await _semaphore.WaitAsync(token);

            if (_cts.IsCancellationRequested)
                return;

            // execute new task
            await ProcessMessage(message, _cts.Token);
        }
        finally { _semaphore.Release(); }
    }

    public void Shutdown()
    {
        _cts.Cancel();
        Task.WhenAll(_tasks).GetAwaiter().GetResult();
    }

    // Override

    public abstract ReadOnlyCollection<Type> MessageTypes { get; }

    protected abstract Task<bool> ProcessMessage(IMessage message, CancellationToken token = default);
}