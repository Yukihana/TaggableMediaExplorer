using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;

namespace TTX.Services.Communications;

public interface IMessageBus
{
    void RegisterService(ServiceBase service);

    int ServicesCount();

    Task Enqueue(IMessage message, CancellationToken token = default);

    Task Enqueue(IEnumerable<IMessage> messages, CancellationToken token = default);

    Task SendCommand(string target, string command, CancellationToken token = default);
}