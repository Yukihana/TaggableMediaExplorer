using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.BaseClasses;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Services.Communications;

public class MessageBus : IMessageBus
{
    private readonly List<ServiceBase> _services = new();

    public async Task Queue(IMessage message, CancellationToken token = default)
    {
        foreach (ServiceBase service in _services)
        {
            if (service.MessageTypes.Contains(message.GetType()))
            {
                await service.TryProcessMessage(message, token);
            }
        }
    }
    public async Task Queue(IEnumerable<IMessage> messages, CancellationToken token = default)
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

    public void RegisterService(ServiceBase service)
    {
        _services.Add(service);
    }
}