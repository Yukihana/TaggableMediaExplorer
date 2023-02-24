using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.Legacy.AssetsIndexer;

public partial class AssetsIndexerService
{
    private async Task ProcessPending(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;

        try
        {
            await _semaphoreCurrent.WaitAsync(token).ConfigureAwait(false);

            if (_processingTask != null && !_processingTask.IsCompleted)
                return;

            _cts = new();
            _processingTask = Task.Run(async () => await ProcessByBatches(_cts.Token).ConfigureAwait(false), token);
        }
        finally { _semaphoreCurrent.Release(); }
    }

    private async Task Reload(CancellationToken ctoken = default)
    {
        try
        {
            Stopwatch timer = Stopwatch.StartNew();

            IsReady = false;

            // Records
            await ReloadRecords(ctoken).ConfigureAwait(false);

            // Fetch Files
            _assetTracking.ClearPending();
            HashSet<string> paths = _assetTracking.GetAllFiles(ctoken);

            // QuickSync and set indexer state to Ready
            IEnumerable<string> pending = await _assetSynchronisation.QuickSync(paths, ctoken).ConfigureAwait(false);
            IsReady = true;

            // Continue with deep sync
            if (pending.Any())
                _ = await _assetSynchronisation.FullSync(pending, ctoken).ConfigureAwait(false);

            // For files that failed, push them onto the next batch (figure out what to do)
            // Change discard to pending.

            // add additional messages for deepsync maybe.

            timer.Stop();
            _logger.LogInformation("Full assets reload completed in {elapsed} ms.", timer.Elapsed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reload failed.");
        }
    }

    private async Task ReloadRecords(CancellationToken ctoken = default)
    {
        Stopwatch timer = Stopwatch.StartNew();
        await _assetSynchronisation.ReloadRecords(ctoken).ConfigureAwait(false);
        timer.Stop();
        _logger.LogInformation("Records loaded in {elapsed} ms.", timer.Elapsed);
    }
}