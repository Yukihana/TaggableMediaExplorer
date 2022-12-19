using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;

namespace TTX.Services.Communications;

public class MessageBus : IMessageBus
{
    private readonly List<ServiceBase> _services = new();

    public async Task Enqueue(IMessage message, CancellationToken token = default)
    {
        foreach (ServiceBase service in _services)
        {
            if (service.MessageTypes.Contains(message.GetType()))
            {
                await service.TryProcessMessage(message, token);
            }
        }
    }

    public async Task Enqueue(IEnumerable<IMessage> messages, CancellationToken token = default)
    {
        List<IMessage> msg = new(messages);
        foreach (ServiceBase service in _services)
        {
            var selected = msg.Where(x => service.MessageTypes.Contains(x.GetType()));
            foreach (IMessage message in selected)
            {
                await service.TryProcessMessage(message, token);
            }
            msg.RemoveAll(x => selected.Contains(x));
        }
    }

    public async Task SendCommand(ServiceCommand command, CancellationToken token = default)
    {
        foreach(ServiceBase service in _services)
        {
            if(service.Identifier.Equals(command.TargetService))
                await service.TryProcessMessage(command, token);
        }
    }

    public void RegisterService(ServiceBase service)
    {
        _services.Add(service);
    }
}