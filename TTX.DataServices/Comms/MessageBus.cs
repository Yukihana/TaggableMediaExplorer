using System.Collections.Generic;
using System.Threading;

namespace TTX.Data.Comms;

internal class MessageBus : IMessageBus
{
    private readonly ReaderWriterLockSlim _lock = new();

    private readonly List<object> _queue = new();

    internal void QueueNext<T>(IMessage message)
    {
        try
        {
            _lock.EnterWriteLock();
            _queue.Add(message);
        }
        finally { _lock.ExitWriteLock(); }
    }

    internal List<T> GetQueued<T>()
    {
        try
        {
            _lock.EnterWriteLock();
            List<T> list = new();
            for (int i = 0; i < _queue.Count; i++)
            {
                if (_queue[i] is T message)
                    list.Add(message);
            }
            for (int i = 0; i < list.Count; i++)
            {
                _queue.Remove(list[i]!);
            }
            return list;
        }
        finally { _lock.ExitWriteLock(); }
    }
}