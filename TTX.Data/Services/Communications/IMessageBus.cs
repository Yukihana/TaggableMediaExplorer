using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Services.Communications;

public interface IMessageBus
{
    Task Queue(IMessage message, CancellationToken token = default);

    Task Queue(IEnumerable<IMessage> messages, CancellationToken token = default);
}