using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.ControlLayer.AssetIndexing;

public interface IAssetIndexingService
{
    Task StartIndexing(CancellationToken ctoken = default);

    Task StopIndexing(CancellationToken ctoken = default);
}