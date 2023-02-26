using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.ControlLayer.AssetIndexing;

public partial class AssetIndexingService
{
    public async partial Task StartIndexing(CancellationToken ctoken)
    {
        try
        {
            await _semaphoreControl.WaitAsync(ctoken).ConfigureAwait(false);

            ctoken.ThrowIfCancellationRequested();

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Disable creating new and flush out existing file system events (Stop, Cancel, Unblock, Await)
            _assetTracking.StopWatcher();
            _cts.Cancel();
            _gate.Set();
            await WaitForSemaphoreClearance(ctoken).ConfigureAwait(false);

            // Once the tasks have been flushed out, prepare a blocking buffer (Token, block, ID, Start)
            CreateNewCancellationTokenSource();
            _gate.Reset();
            ChangeGateId();
            _assetTracking.StartWatcher(OnFileSystemEvent);

            // Execute reload with local CancellationToken
            await Reload(_cts.Token).ConfigureAwait(false);

            // Unblock incoming events
            _gate.Set();

            // Summarize
            stopwatch.Stop();
            _logger.LogInformation("Reload completed successfully in {elapsed}", stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start indexing.");
        }
        finally { _semaphoreControl.Release(); }
    }

    public async partial Task StopIndexing(CancellationToken ctoken)
    {
        try
        {
            await _semaphoreControl.WaitAsync(ctoken).ConfigureAwait(false);

            ctoken.ThrowIfCancellationRequested();

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Cancel via token
            _cts.Cancel();

            // Ensure tasks cancel themselves instead of being held
            _gate.Set();

            // Wait for all tasks to finish
            await WaitForSemaphoreClearance(ctoken).ConfigureAwait(false);

            stopwatch.Stop();
            _logger.LogInformation("Indexing stopped successfully in {elapsed}.", stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to stop indexing.");
        }
        finally { _semaphoreControl.Release(); }
    }
}