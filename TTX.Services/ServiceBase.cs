using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;
using TTX.Library.TaskManagement;
using TTX.Services.Communications;

namespace TTX.Services;

public abstract class ServiceBase
{
    public abstract string Identifier { get; }

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
        if (token.IsCancellationRequested)
            return;

        await _taskQueue.Add(new Task(async () => await ProcessMessage(message, _taskQueue.Token)));
    }

    // Override

    protected abstract Task ProcessMessage(IMessage message, CancellationToken token = default);
}