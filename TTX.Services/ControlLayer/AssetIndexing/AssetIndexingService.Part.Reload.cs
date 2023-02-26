using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.ControlLayer.AssetIndexing;

public partial class AssetIndexingService
{
    private async partial Task Reload(CancellationToken ctoken)
    {
        try
        {
            Stopwatch timer = Stopwatch.StartNew();

            // Reset presences and scan the database. Repair if necessary.
            await ResetRepair(ctoken).ConfigureAwait(false);

            // Fetch Files
            string[] paths = _assetTracking.GetAllFiles(ctoken);

            // QuickSync
            if (paths.Any())
                paths = (await QuickSyncMultiple(paths, ctoken).ConfigureAwait(false)).ToArray();

            // FullSync
            if (paths.Any())
                await FullSyncMultiple(paths, ctoken).ConfigureAwait(false);

            timer.Stop();
            _logger.LogInformation("Reload completed in {elapsed} ms.", timer.Elapsed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reload failed.");
        }
    }

    private async Task ResetRepair(CancellationToken ctoken = default)
    {
        Stopwatch timer = Stopwatch.StartNew();
        await _assetSynchronisation.ResetRepair(ctoken).ConfigureAwait(false);
        timer.Stop();
        _logger.LogInformation("Stage1of3:Preparation completed in {elapsed} ms.", timer.Elapsed);
    }

    private async Task<IEnumerable<string>> QuickSyncMultiple(IEnumerable<string> paths, CancellationToken ctoken)
    {
        Stopwatch timer = Stopwatch.StartNew();
        try
        {
            return await _assetSynchronisation.QuickSync(paths, ctoken).ConfigureAwait(false);
        }
        finally
        {
            timer.Stop();
            _logger.LogInformation("Stage2of3:QuickSync completed in {elapsed} ms.", timer.Elapsed);
        }
    }

    private async Task FullSyncMultiple(IEnumerable<string> paths, CancellationToken ctoken)
    {
        Stopwatch timer = Stopwatch.StartNew();

        try
        {
            int total = paths.Count();
            int successes = 0;

            await Parallel.ForEachAsync(paths, ctoken, async (path, token) =>
            {
                if (await _assetSynchronisation.FullSync(path, isReloadSync: true, token).ConfigureAwait(false))
                    Interlocked.Increment(ref successes);
            }).ConfigureAwait(false);

            if (successes == total)
                _logger.LogInformation("Fully synchronised {successes} out of {total} assets.", successes, total);
            else
                _logger.LogWarning("Fully synchronised {successes} out of {total} assets.", successes, total);
        }
        finally
        {
            timer.Stop();
            _logger.LogInformation("Stage3of3:FullSync completed in {elapsed} ms.", timer.Elapsed);
        }
    }
}