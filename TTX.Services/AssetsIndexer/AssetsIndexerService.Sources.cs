using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;
using TTX.Library.Helpers;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    public async Task<HashedAssetFile?> GetHashedMetadata(string path, CancellationToken token = default)
    {
        try
        {
            AssetFile? file = await _assetInfo.Fetch(path, token).ConfigureAwait(false);
            if (file == null)
                throw new Exception("Metadata fetch failed");

            HashedAssetFile hfile = file.CopyValues<HashedAssetFile>();
            byte[]? SHA2 = await _assetInfo.ComputeSHA256Async(file, token).ConfigureAwait(false);

            if (SHA2 == null)
                throw new Exception("Checksum calculation failed");

            return hfile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to generate hashed file info for {path}", path);
        }
        return null;
    }

    // Watcher CRUD Source

    public async void OnCreated(object sender, FileSystemEventArgs e)
        => await EnqueuePending(new()
        {
            FullPath = e.FullPath,
            UpdateType = WatcherUpdateType.Created
        }).ConfigureAwait(false);

    public async void OnRenamed(object sender, RenamedEventArgs e)
        => await EnqueuePending(new()
        {
            FullPath = e.FullPath,
            OldPath = e.OldFullPath,
            UpdateType = WatcherUpdateType.Moved
        }).ConfigureAwait(false);

    public async void OnChanged(object sender, FileSystemEventArgs e)
        => await EnqueuePending(new()
        {
            FullPath = e.FullPath,
            UpdateType = WatcherUpdateType.Modified
        }).ConfigureAwait(false);

    public async void OnDeleted(object sender, FileSystemEventArgs e)
        => await EnqueuePending(new()
        {
            FullPath = e.FullPath,
            UpdateType = WatcherUpdateType.Deleted
        }).ConfigureAwait(false);
}