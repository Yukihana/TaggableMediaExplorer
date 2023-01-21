using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;
using TTX.Library.Helpers;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly HashSet<AssetFile> _cachedMetadata = new();

    private async Task Reload(CancellationToken token = default)
    {
        Stopwatch timer = Stopwatch.StartNew();

        IsReady = false;

        // Records
        await ReloadRecords(token).ConfigureAwait(false);

        // Fetch Files
        await ClearPending(token).ConfigureAwait(false);
        HashSet<string> paths = await _watcher.GetAllFiles(token).ConfigureAwait(false);

        // QuickSync and set indexer state to Ready
        HashSet<string> pending = await QuickSyncFiles(paths, token).ConfigureAwait(false);
        IsReady = true;

        // Continue with deep sync
        await DeepSyncFiles(pending, token).ConfigureAwait(false);

        timer.Stop();
        _logger.LogInformation("Full assets reload completed in {elapsed} ms.", timer.Elapsed);
    }

    private async Task<HashSet<string>> QuickSyncFiles(HashSet<string> paths, CancellationToken token = default)
    {
        // Metadata
        List<AssetFile> files = await _assetInfo.Fetch(paths, token).ConfigureAwait(false);
        _logger.LogInformation("Loaded metadata for {loaded} out of {total} files.", files.Count, paths.Count);

        // Cache
        files.AddTo(_cachedMetadata);
        HashSet<string> pending = paths.ToHashSet();

        // Provisional Sync
        int n = 0;
        foreach (AssetFile file in files)
        {
            if (token.IsCancellationRequested)
                break;

            if (await QuickMatch(file, token).ConfigureAwait(false))
            {
                n++;
                pending.Remove(file.FullPath);
                _cachedMetadata.Remove(file);
            }
        }
        _logger.LogInformation("Provisionally synced {synced} out of {total} files.", n, files.Count);

        return pending;
    }

    private async Task DeepSyncFiles(IEnumerable<string> paths, CancellationToken token = default)
    {
        // Grab metadatas
        HashSet<string> pending = paths.Where(p => !_cachedMetadata.Any(m => m.FullPath.Equals(p))).ToHashSet();
        HashSet<AssetFile> metadatas = _cachedMetadata.ToHashSet();
        _cachedMetadata.Clear();
        (await _assetInfo.Fetch(pending, token).ConfigureAwait(false)).AddTo(metadatas);

        // Hash
        HashSet<HashedAssetFile> hashedFiles = new();

        //
    }
}