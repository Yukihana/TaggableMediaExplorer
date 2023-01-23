using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Extensions;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private async Task Reload(CancellationToken token = default)
    {
        Stopwatch timer = Stopwatch.StartNew();

        IsReady = false;

        // Records
        await ReloadRecords(token).ConfigureAwait(false);

        // Mark records with matching asset integrity data (as this may cause duplication issues during sync)
        ScanDuplicateRecords();

        // Fetch Files
        ClearPending();
        HashSet<string> paths = await _watcher.GetAllFiles(token).ConfigureAwait(false);

        // QuickSync and set indexer state to Ready
        HashSet<string> pending = await QuickSyncFiles(paths, token).ConfigureAwait(false);
        IsReady = true;

        // Continue with deep sync
        await ProcessBatch(pending.ToArray(), token).ConfigureAwait(false);

        timer.Stop();
        _logger.LogInformation("Full assets reload completed in {elapsed} ms.", timer.Elapsed);
    }

    private void ScanDuplicateRecords()
    {
        ConcurrentBag<(string, AssetRecord)> unordered = new();
        Parallel.ForEach(Snapshot(), x => unordered.Add((x.ToIdentityString(), x)));

        HashSet<string> identities = new();
        HashSet<string> duplicateidentities = new();

        foreach ((string, AssetRecord) element in unordered)
        {
            if (!identities.Add(element.Item1))
                duplicateidentities.Add(element.Item1);
        }

        foreach ((string, AssetRecord) element in unordered)
        {
            if (duplicateidentities.Contains(element.Item1))
                _auxiliary.AddDuplicateRecords(element.Item1, element.Item2);
        }
    }

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
        List<AssetRecord> recs = Snapshot();
        string localPath = Path.GetRelativePath(_options.AssetsPathFull, path);
        AssetFile? file = await _assetInfo.Fetch(path, false, token).ConfigureAwait(false);
        if (file == null)
            return false;

        // Match
        ConcurrentBag<AssetRecord> matchedBag = new();
        Parallel.ForEach(recs, rec =>
        {
            if (ProvisionalMatch(rec, file, localPath))
                matchedBag.Add(rec);
        });
        List<AssetRecord> matched = matchedBag.ToHashSet().ToList();

        // If too many matching records, fail. Possible duplicate record. Hashed sync required.
        if (matched.Count != 1)
            return false;

        // If already validated, conclude as failed. Possible duplicate file. Hashed sync required
        return matched[0].TryValidate();
    }
}