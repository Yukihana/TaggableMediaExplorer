using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Extensions;

namespace TTX.Services.Legacy.AssetsIndexer;

public partial class AssetsIndexerService
{
    private async Task Reload(CancellationToken ctoken = default)
    {
        Stopwatch timer = Stopwatch.StartNew();

        IsReady = false;

        // Records
        await ReloadRecords(ctoken).ConfigureAwait(false);

        // Mark records with matching asset integrity data (as this may cause duplication issues during sync)
        ScanDuplicateRecords();

        // Fetch Files
        _assetTracking.ClearPending();
        HashSet<string> paths = _assetTracking.GetAllFiles(ctoken);

        // QuickSync and set indexer state to Ready
        HashSet<string> pending = await QuickSyncFiles(paths, ctoken).ConfigureAwait(false);
        IsReady = true;

        // Continue with deep sync
        if (pending.Count > 0)
            await DeepSync(pending.ToArray(), ctoken).ConfigureAwait(false);

        timer.Stop();
        _logger.LogInformation("Full assets reload completed in {elapsed} ms.", timer.Elapsed);
    }

    private void ScanDuplicateRecords()
    {
        ConcurrentBag<(string, AssetRecord)> unordered = new();
        Parallel.ForEach(_assetDatabase.Snapshot(), x => unordered.Add((x.ToIdentityString(), x)));

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

    private async Task ReloadRecords(CancellationToken token = default)
    {
        Stopwatch timer = Stopwatch.StartNew();
        await _assetDatabase.Reload(token).ConfigureAwait(false);
        timer.Stop();
        _logger.LogInformation("Records loaded in {elapsed} ms.", timer.Elapsed);
    }
}