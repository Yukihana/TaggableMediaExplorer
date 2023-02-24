using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.Legacy.AssetsIndexer;

public interface IAssetsIndexerService
{
    // Api

    bool IsReady { get; }

    // Control panel

    Task StartIndexing(CancellationToken token = default);

    Task StopIndexing(CancellationToken token = default);
}