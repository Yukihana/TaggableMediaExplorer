using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Library.TaskManagement;

public class TaskQueue
{
    private readonly SemaphoreSlim _semaphoreConcurrency, _semaphoreQueue = new(1);
    private readonly List<Task> _queue = new();
    private CancellationTokenSource _cts = new();

    // Properties

    public CancellationToken Token => _cts.Token;

    public void Cancel() => _cts.Cancel();

    public void RenewCancellationTokenSource() => _cts = new();

    public async Task WaitForFinish() => await Task.WhenAll(_queue);

    // public

    public TaskQueue(int concurrent)
    {
        _semaphoreConcurrency = new(concurrent);
    }

    public async Task<bool> Add(Task task)
    {
        if (task.Status != TaskStatus.Created)
            return false;

        Task wrapper = CreateTask(task);
        try
        {
            await _semaphoreQueue.WaitAsync();
            _queue.Add(wrapper);
        }
        finally { _semaphoreQueue.Release(); }

        return true;
    }

    private Task CreateTask(Task task)
    {
        Task wrapper = new(async () => await TaskWaiter(task, _cts.Token));
        wrapper.Start();
        wrapper.ContinueWith(async (x) =>
        {
            try
            {
                await _semaphoreQueue.WaitAsync();
                _queue.Remove(x);
            }
            finally { _semaphoreQueue.Release(); }
        });
        return wrapper;
    }

    // private

    private async Task TaskWaiter(Task task, CancellationToken token = default)
    {
        try
        {
            await _semaphoreConcurrency.WaitAsync(token);

            if (_cts.IsCancellationRequested)
                return;

            task.Start();
            await task;
        }
        finally { _semaphoreConcurrency.Release(); }
    }
}