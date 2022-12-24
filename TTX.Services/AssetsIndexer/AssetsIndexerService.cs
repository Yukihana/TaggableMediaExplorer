using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;
using TTX.Services.AssetsIndexer;
using TTX.Services.Communications;

namespace TTX.Services.Indexer;

/// <summary>
/// Storage class for AssetInfo entities.
/// </summary>
public class AssetsIndexerService : ServiceBase, IAssetsIndexerService
{
    private readonly IAssetsIndexerOptions _options;
    public override string Identifier => _options.AssetsIndexerSID;

    public AssetsIndexerService(IMessageBus bus, IAssetsIndexerOptions options) : base(bus, 1)
    {
        _options = options;
    }

    protected override Task ProcessMessage(IMessage message, CancellationToken token = default)
    {
        throw new System.NotImplementedException();
    }
}