using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public interface IAssetSynchronisationService
{
    Task ReloadRecords(CancellationToken ctoken = default);

    Task<IEnumerable<string>> QuickSync(IEnumerable<string> paths, CancellationToken ctoken = default);

    Task<IEnumerable<string>> FullSync(IEnumerable<string> paths, CancellationToken ctoken = default);
}