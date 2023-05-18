using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;
using TTX.Library.Helpers.EnumerableHelpers;
using TTX.Library.InstancingHelpers;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService
{
    private async Task<bool> ConfirmAssetAbsence(string path, CancellationToken ctoken)
    {
        // Wrap it in the semaphore to prevent invalidation if concurrently evaluating another synchronisation event on the same path
        try
        {
            await _semaphore.WaitAsync(ctoken).ConfigureAwait(false);
            _assetPresence.Remove(Path.GetRelativePath(_options.AssetsPathFull, path));
        }
        finally { _semaphore.Release(); }

        // If file exists, queue for retry
        if (await _assetAnalysis.FileExists(path, ctoken).ConfigureAwait(false))
        {
            _logger.LogError("Unable to read file. Invalidating and requeueing for next batch: {path}", path);
            return false;
        }

        // Else, consider it handled
        _logger.LogWarning("Asset is no longer available at: {path}", path);
        return true;
    }

    private async Task<bool> TryMatchByData(FullAssetSyncInfo syncInfo, AssetMediaInfo mediaInfo, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        AssetRecord? untracked = null;
        await _assetDatabase.WriteAsync(async (assets, ct) =>
        {
            AssetRecord[] matches = await assets
                .Where(asset =>
                    asset.SizeBytes == syncInfo.SizeBytes &&
                    asset.Crumbs.SequenceEqual(syncInfo.Crumbs) &&
                    asset.SHA256.SequenceEqual(syncInfo.SHA256))
                .ToArrayAsync(ctoken)
                .ConfigureAwait(false);
            AssetRecord? match = matches.SelectOneNoneOrThrow($"Finding an integrity match for {syncInfo.LocalPath}. ");

            if (match is null)
                return false;

            // If a match is found

            // Update dates
            match.LocalPath = syncInfo.LocalPath;
            match.CreatedUtc = syncInfo.CreatedUtc;
            match.ModifiedUtc = syncInfo.ModifiedUtc;
            match.VerifiedUtc = syncInfo.VerifiedUtc;
            match.UpdatedUtc = DateTime.UtcNow;

            // Update media info
            mediaInfo.CopyConstrainedTo<IMediaInfo>(match);

            // Create an untracked copy for postsync and update the database
            untracked = match.DeserializedCopy();
            return true;
        }, ctoken).ConfigureAwait(false);

        if (untracked is null)
            return false;

        // Execute post sync operations
        await OnSyncSuccess(untracked, ctoken).ConfigureAwait(false);
        return true;
    }

    /*
    private async Task<bool> FindMatchByHealthCheck(FullAssetSyncInfo syncInfo, CancellationToken ctoken)
    {
        // TODO after hashed fragments is implemented
        //
        // If health signature by hashed-fragments is over 90% match:
        // _logger.LogInformation("Sync mismatch. Change detected at {path}", syncInfo.LocalPath);
        // set it as handled to prevent creating a new record of a broken file
    }
    */

    private async Task TryCreateFromSyncInfo(FullAssetSyncInfo syncInfo, AssetMediaInfo mediaInfo, CancellationToken ctoken)
    {
        // TODO figure out how to add mediaAnalysis
        // Create record first, then send to _assetDatabase.

        AssetRecord newRecord = await _assetDatabase.Create(syncInfo, mediaInfo, ctoken).ConfigureAwait(false);
        await OnSyncSuccess(newRecord, ctoken).ConfigureAwait(false);
    }
}