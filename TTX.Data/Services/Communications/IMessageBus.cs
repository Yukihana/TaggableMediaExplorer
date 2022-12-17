using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Services.Communications;

public interface IMessageBus
{
    Task Enqueue(IMessage message, CancellationToken token = default);

    Task Enqueue(IEnumerable<IMessage> messages, CancellationToken token = default);
}