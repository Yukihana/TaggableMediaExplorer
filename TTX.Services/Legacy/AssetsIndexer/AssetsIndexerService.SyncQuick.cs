using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.Legacy.AssetsIndexer;

public partial class AssetsIndexerService
{
    private async Task<HashSet<string>> QuickSyncFiles(HashSet<string> paths, CancellationToken token = default)
    {
        ConcurrentBag<string> pending = new();
        uint success = 0;

        await Parallel.ForEachAsync(paths, token,
            async (path, token) =>
            {
                if (await QuickSync(path, token).ConfigureAwait(false))
                    Interlocked.Increment(ref success);
                else
                    pending.Add(path);
            }).ConfigureAwait(false);

        _logger.LogInformation("Provisionally synced {synced} out of {total} files.", success, paths.Count);
        return pending.ToHashSet();
    }

    private async Task<bool> QuickSync(string path, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        // Prepare
        List<AssetRecord> recs = _assetDatabase.Snapshot();
        QuickAssetSyncInfo? info = await _assetAnalysis.Fetch(path, _options.AssetsPathFull, token).ConfigureAwait(false);
        if (info == null)
            return false;

        // Match
        ConcurrentBag<AssetRecord> matchedBag = new();
        Parallel.ForEach(recs, rec =>
        {
            if (ProvisionalMatch(rec, info))
                matchedBag.Add(rec);
        });
        List<AssetRecord> matched = matchedBag.ToHashSet().ToList();

        // If too many matching records, fail. Possible duplicate record. Hashed sync required.
        if (matched.Count != 1)
            return false;

        // Finally register the asset on the presence registry
        _assetPresence.Set(info.LocalPath, matched[0].ItemId);
        return true;
    }
}