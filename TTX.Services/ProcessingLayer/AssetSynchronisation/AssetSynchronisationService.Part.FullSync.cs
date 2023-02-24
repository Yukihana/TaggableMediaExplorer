using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService
{
    public async partial Task<IEnumerable<string>> FullSync(IEnumerable<string> paths, CancellationToken ctoken)
    {
        Stopwatch timer = Stopwatch.StartNew();
        int pathCount = paths.Count();
        int successCount = 0;
        ConcurrentBag<string> pending = new();

        await Parallel.ForEachAsync(paths, ctoken, async (path, token) =>
        {
            if (!await ProcessFile(path, token).ConfigureAwait(false))
            {
                _logger.LogError("Failed to sync file {path}", path);
                pending.Add(path);
            }
            else Interlocked.Increment(ref successCount);
        }).ConfigureAwait(false);
        timer.Stop();
        _logger.LogInformation("Batch processed in {elapsed} ms. No of files: {pathCount}.", timer.Elapsed, pathCount);
        if (successCount < pathCount)
            _logger.LogWarning("Failed to sync {failCount} files.", pathCount - successCount);

        return pending;
    }

    // Add logger messages for every outcome
    private async Task<bool> ProcessFile(string path, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        // Prepare
        List<AssetRecord> recs = _assetDatabase.Snapshot();
        bool fileExists = await _assetAnalysis.FileExists(path, token).ConfigureAwait(false);
        string localPath = Path.GetRelativePath(_options.AssetsPathFull, path);
        FullAssetSyncInfo? file = null;
        if (fileExists)
            file = await _assetAnalysis.FetchHashed(path, _options.AssetsPathFull, token).ConfigureAwait(false);

        // Start sync
        try
        {
            await _semaphore.WaitAsync(token).ConfigureAwait(false);

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
                    await _assetDatabase.Update(dataMatch.ItemId, x => x.LocalPath = localPath, token).ConfigureAwait(false);
                    _logger.LogInformation("Updated record for file: {path}", path);

                    if (_assetPresence.GetAll(dataMatch.ItemId).Length > 1)
                        _logger.LogInformation("Found duplicate for Id:{itemId} at {path}", dataMatch.ItemId, path);
                }
                return true;
            }

            // Check for modified
            if (FindMatchByPath(localPath, recs).Count > 0)
            {
                //_auxiliary.AddModifiedFiles(path);
                _logger.LogInformation("Sync mismatch. Change detected at {path}", path);
                return true;
            }

            // If not already exited, then create a new record, append to local memory
            if (await _assetDatabase.Create(file, token).ConfigureAwait(false) is byte[] itemId)
            {
                _assetPresence.Set(localPath, itemId);
                return true;
            }

            // If creation failed, return false.
            return false;
        }
        finally { _semaphore.Release(); }
    }
}