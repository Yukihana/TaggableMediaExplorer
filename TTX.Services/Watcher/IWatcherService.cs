using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Services.AssetsIndexer;

namespace TTX.Services.Watcher;

public interface IWatcherService
{
    Task<HashSet<string>> GetAllFiles(CancellationToken token = default);

    // FSW

    void StartWatcher(IAssetsIndexerService indexer);

    void StopWatcher();
}