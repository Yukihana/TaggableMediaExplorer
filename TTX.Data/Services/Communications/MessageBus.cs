using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.BaseClasses;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Services.Communications;

public class MessageBus : IMessageBus
{
    private readonly SemaphoreSlim _gate = new(1);

    private readonly List<IMessage> _queue = new();


    public void RegisterService(ServiceBase service, Type[] messageTypes)
    {

    }


    private async Task QueueTask(IMessage message)
    {
        foreach(MessageProcessor service in MessageProcessors)
        {
            if(service.MessageTypes.Contains(message.GetType()))
            {
                service.
            }
        }
    }




    public async Task Queue(IMessage message)
    {
        try
        {
            await _gate.WaitAsync().ConfigureAwait(false);
            _queue.Add(message);
        }
        finally { _gate.Release(); }
    }

    public async Task Queue(IEnumerable<IMessage> messages)
    {
        try
        {
            await _gate.WaitAsync().ConfigureAwait(false);
            foreach (IMessage message in messages)
            {
                _queue.Add(message);
            }
        }
        finally { _gate.Release(); }
    }

    public async Task<T?> GetNextQueued<T>() where T : struct, IMessage
    {
        try
        {
            await _gate.WaitAsync().ConfigureAwait(false);
            for (int i = 0; i < _queue.Count; i++)
            {
                if (_queue[i] is T message)
                    return message;
            }
            return null;
        }
        finally { _gate.Release(); }
    }

    public async Task Unqueue(IMessage message)
    {
        try
        {
            await _gate.WaitAsync().ConfigureAwait(false);
            if (_queue.Contains(message))
                _queue.Remove(message);
        }
        finally { _gate.Release(); }
    }
}