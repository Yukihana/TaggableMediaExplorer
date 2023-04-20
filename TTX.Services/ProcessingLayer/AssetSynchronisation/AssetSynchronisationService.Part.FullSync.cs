using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService
{
    public async partial Task<bool> FullSync(string path, bool isReloadSync, CancellationToken ctoken)
    {
        TimeSpan baseRetryInterval = _options.AssetSyncAttemptBaseInterval;
        int maxAttempts = isReloadSync ? _options.AssetFullSyncAttemptsOnReload : _options.AssetFullSyncAttemptsOnEvent;

        bool handled = false;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            ctoken.ThrowIfCancellationRequested();

            if (await AttemptFullSync(path, ctoken).ConfigureAwait(false))
            {
                _logger.LogInformation("Successfully synchronized {path}, Attempts: {attempts}", path, attempts + 1);
                handled = true;
                break;
            }

            _logger.LogError("Failed attempt:{attempts} of trying to synchronise {path}.", ++attempts, path);

            // After increment, to ensure no wait after last attempt
            if (attempts < maxAttempts)
                await Task.Delay(baseRetryInterval * attempts, ctoken).ConfigureAwait(false);
        }

        if (!handled)
            _logger.LogError("Maximum attempts of {maxAttempts} reached. Unable to synchronise asset at {path}.", maxAttempts, path);

        return handled;
    }

    private async partial Task<bool> AttemptFullSync(string path, CancellationToken ctoken)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            // Access denied
            if (await _assetAnalysis.FetchHashed(path, _options.AssetsPathFull, ctoken).ConfigureAwait(false) is not FullAssetSyncInfo syncInfo)
                return await ConfirmAssetAbsence(path, ctoken).ConfigureAwait(false);

            // Additionally fetch media analysis info to avoid long pause during database operations
            AssetMediaInfo mediaInfo = await _mediaCodec.GetMediaAnalysisInfo(path, ctoken).ConfigureAwait(false);

            return await SyncByInfo(syncInfo, mediaInfo, ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Encountered an error trying to sync {path}. Message: {message}", path, ex.Message);
            return false;
        }
    }

    private async Task<bool> SyncByInfo(FullAssetSyncInfo syncInfo, AssetMediaInfo mediaInfo, CancellationToken ctoken = default)
    {
        try
        {
            await _semaphore.WaitAsync(ctoken).ConfigureAwait(false);

            _logger.LogInformation("Trying to sync {path} using its data integrity signature...", syncInfo.LocalPath);
            if (await TryMatchByData(syncInfo, mediaInfo, ctoken).ConfigureAwait(false))
                return true;

            // TODO: Add file health check after adding fragmented analysis hashes
            // FindMatchByHealthCheck

            _logger.LogInformation("No matches found. Creating a new record for {path}", syncInfo.LocalPath);
            await TryCreateFromSyncInfo(syncInfo, mediaInfo, ctoken).ConfigureAwait(false);
            return true;
        }
        finally { _semaphore.Release(); }
    }
}