using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly SemaphoreSlim _semaphoreSync = new(1);

    private async Task ProcessUpdate(WatcherUpdate update, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;

        // Fetch file hash first
        //HashedAssetFile hashed = await _integrity.ComputeSHA256Async()

        bool result = update.UpdateType switch
        {
            WatcherUpdateType.Created => await FileCreated(update.FullPath, token).ConfigureAwait(false),
            WatcherUpdateType.Moved => await FileMoved(update.FullPath, update.OldPath, token).ConfigureAwait(false),
            WatcherUpdateType.Modified => await FileModified(update.FullPath, token).ConfigureAwait(false),
            WatcherUpdateType.Deleted => await FileDeleted(update.FullPath, token).ConfigureAwait(false),
            _ => await ProcessFile(update.FullPath, token).ConfigureAwait(false)
        };

        // On fail, run unknown on new (and old if applicable)

        // Fix this (setup semaphore for _procTask access)
        // Add queue analysis semaphore to ensure single compare at a time (_semaphoreCompare?)
        // Semaphore compare should be in the comparison processes partial

        if (!result)
        {
            if (update.UpdateType != WatcherUpdateType.Unknown)
            {
                update.UpdateType = WatcherUpdateType.Unknown;
                await EnqueuePending(update, token).ConfigureAwait(false);
            }
            else
                _logger.LogError("Failed to sync file {path}", update.FullPath);
        }

        // finally wrap up with log on total failure
    }


    // Created

    private async Task<bool> FileCreated(string path, HashedAssetFile? file, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        // create hash
        HashedAssetFile? hdata = await GetHashedMetadata(path, token).ConfigureAwait(false);
        if (hdata == null)
            return false;

        // if exists : activate or add to duplicates
        foreach(AssetRecord rec in await Snapshot(token).ConfigureAwait(false))
        {
            if (await DataMatch(rec, hdata, token))
            {
                await AddToExisting();
                // if invalid, move location to duplicate, then update location to this

                // if valid, add location to duplicate

                return true;
            }

            // do 90% health check on all matching files
            if (await PartialMatch(rec, hdata, token))
            {
                // log this to admin messages

                return true;
            }
        }

        // else create
        await CreateRecord(hdata, token).ConfigureAwait(false);
        return true;
    }

    // Moved

    private async Task<bool> FileMoved(string path, string oldPath, HashedAssetFile? file, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        // check if old file is deleted, do delete

        // Get record by old path

        // check if file exists at new path, if so hash it

        // deep match and update record.

        try
        {
            await _semaphoreSync.WaitAsync(token).ConfigureAwait(false);


        }
        finally { _semaphoreSync.Release(); }

        return false;
    }

    // Modified

    private async Task<bool> FileModified(string path, HashedAssetFile? file, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        try
        {
            await _semaphoreSync.WaitAsync(token).ConfigureAwait(false);


        }
        finally { _semaphoreSync.Release(); }

        return false;
    }

    // Deleted

    private async Task<bool> FileDeleted(string path, HashedAssetFile? file, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        try
        {
            await _semaphoreSync.WaitAsync(token).ConfigureAwait(false);


        }
        finally { _semaphoreSync.Release(); }

        return false;
    }

    // Unknown

    private async Task<bool> ProcessFile(string path, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        try
        {
            await _semaphoreCurrent.WaitAsync(token).ConfigureAwait(false);

            // Generate Metadata
            AssetFile file = new();

            // Generate Hash
            HashedAssetFile hfile = new();

            // Fetch invalids
            HashSet<AssetRecord> recs = await Snapshot(token).ConfigureAwait(false);

            // Match by: Size, Crumbs, Hash, Filename => Instant match

            //

            // If all else fails create new record
            await CreateRecord(hfile, token).ConfigureAwait(false);
        }
        finally { _semaphoreCurrent.Release(); }

        return false;
    }
}