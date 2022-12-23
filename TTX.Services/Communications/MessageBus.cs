using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;

namespace TTX.Services.Communications;

public class MessageBus : IMessageBus
{
    private readonly List<ServiceBase> _services = new();

    public void RegisterService(ServiceBase service)
    {
        _services.Add(service);
    }

    public int ServicesCount()
        => _services.Count;

    public async Task Enqueue(IMessage message, CancellationToken token = default)
    {
        foreach (ServiceBase service in _services)
        {
            if (service.Identifier.Equals(message.TargetSID))
            {
                await service.TryProcessMessage(message, token);
            }
        }
    }

    public async Task Enqueue(IEnumerable<IMessage> messages, CancellationToken token = default)
    {
        foreach (IMessage message in messages)
            await Enqueue(message, token);
    }

    public async Task SendCommand(string service, string command, CancellationToken token = default)
    {
        var sc = new ServiceCommand()
        {
            TargetSID = service,
            CommandString = command
        };
        await Enqueue(sc, token);
    }
}