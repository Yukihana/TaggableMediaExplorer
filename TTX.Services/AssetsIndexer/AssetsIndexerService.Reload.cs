using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    public async Task Reload()
    {
        await RefreshRecords();
        await SyncFiles();
    }

    private async Task RefreshRecords(CancellationToken token = default)
    {
        List<AssetInfo> loaded = await _dbsync.LoadAssets(token);
        try
        {
            await _semaphore.WaitAsync(token);
            _liveSet.Clear();
            _stashed.Clear();
            for (int i = 0; i < loaded.Count; i++)
                _stashed.Add(loaded[i]);
        }
        finally { _semaphore.Release(); }
    }

    private async Task SyncFiles(CancellationToken token = default)
    {
        HashSet<string> paths = await _watcher.GetAllFiles(token);
        List<AssetFile> files = await _metadata.Fetch(paths, token);
    }
}