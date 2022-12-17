using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Services.Communications;
using TTX.Data.Shared.Library;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Shared.BaseClasses;

public abstract class ServiceBase
{
    protected readonly IMessageBus _bus;

    private readonly TaskQueue _taskQueue;

    public ServiceBase(IMessageBus bus, int concurrent)
    {
        _bus = bus;
        _taskQueue = new TaskQueue(concurrent);
    }

    public void Shutdown()
    {
        _taskQueue.Cancel();
        _taskQueue.WaitForFinish().GetAwaiter().GetResult();
    }

    // Send

    protected async Task SendMessage(IMessage message, CancellationToken token = default)
        => await _bus.Enqueue(message, token);

    // Receive

    public async Task TryProcessMessage(IMessage message, CancellationToken token = default)
    {
        if (!MessageTypes.Contains(message.GetType()))
            return;

        if (token.IsCancellationRequested)
            return;

        await _taskQueue.Add(new Task(async () => await ProcessMessage(message, _taskQueue.Token)));
    }

    // Override

    public abstract HashSet<Type> MessageTypes { get; }

    protected abstract Task ProcessMessage(IMessage message, CancellationToken token = default);
}