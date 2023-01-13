using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    public async Task Reload()
    {
        IsReady = false;
        Stopwatch timer = Stopwatch.StartNew();

        await RefreshRecords();
        await SyncFiles();

        timer.Stop();
        IsReady = true;
        
        _logger.LogInformation("Reload completed. Time elapsed elapsed.", timer.Elapsed);
    }

    private async Task RefreshRecords(CancellationToken token = default)
    {
        List<AssetRecord> loaded = await _dbsync.LoadAssets(token);
        try
        {
            await _semaphore.WaitAsync(token);
            _records.Clear();
            for (int i = 0; i < loaded.Count; i++)
                _records.Add(loaded[i]);
        }
        finally { _semaphore.Release(); }
    }

    private async Task SyncFiles(CancellationToken token = default)
    {
        HashSet<string> paths = await _watcher.GetAllFiles(token);
        List<AssetFile> files = await _metadata.Fetch(paths, token);

        await SyncProvisionally(files, token);
    }
}