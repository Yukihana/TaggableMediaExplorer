using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.Legacy.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly SemaphoreSlim _semaphoreSync = new(1);

    // Add logger messages for every outcome
    private async Task<bool> ProcessFile(string path, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        // Prepare
        List<AssetRecord> recs = Snapshot();
        bool fileExists = await _assetAnalysis.FileExists(path, token).ConfigureAwait(false);
        string localPath = Path.GetRelativePath(_options.AssetsPathFull, path);
        FullAssetSyncInfo? file = null;
        if (fileExists)
            file = await _assetAnalysis.FetchHashed(path, _options.AssetsPathFull, token).ConfigureAwait(false);

        // Start sync
        try
        {
            await _semaphoreSync.WaitAsync(token).ConfigureAwait(false);

            // If file is inaccessible, invalidate
            if (file == null)
            {
                _assetPresence.Remove(localPath);

                if (fileExists)
                {
                    _logger.LogError("Unable to read file. Invalidating and requeueing for next batch: {path}", path);
                    return false;
                }

                _logger.LogWarning("Invalidating removed file: {path}", path);
                return true;
            }

            // Get first content match, validate or duplicate
            if (FindMatchByDataIntegrity(file, recs) is AssetRecord dataMatch)
            {
                if (PathMatch(localPath, dataMatch))
                {
                    _assetPresence.Set(localPath, dataMatch.ItemId);
                    _logger.LogInformation("Activated existing record for file: {path}", path);
                }
                else
                {
                    // TODO add old path for log
                    await UpdateRecord(dataMatch, x => x.FilePath = localPath, token).ConfigureAwait(false);
                    _logger.LogInformation("Updated record for file: {path}", path);

                    if (_assetPresence.GetAll(dataMatch.ItemId).Length > 1)
                        _logger.LogInformation("Found duplicate for Id:{itemId} at {path}", dataMatch.ItemId, path);
                }
                return true;
            }

            // Check for modified
            if (FindMatchByPath(localPath, recs).Count > 0)
            {
                _auxiliary.AddModifiedFiles(path);
                _logger.LogInformation("Sync mismatch. Change detected at {path}", path);
                return true;
            }

            // If not already exited, then create a new record, append to local memory
            return await CreateRecord(file, localPath, token).ConfigureAwait(false);
        }
        finally { _semaphoreSync.Release(); }
    }
}