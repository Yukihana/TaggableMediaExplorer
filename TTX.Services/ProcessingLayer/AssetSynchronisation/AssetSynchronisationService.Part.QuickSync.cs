using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;
using TTX.Library.EnumerableHelpers;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService
{
    public async partial Task<IEnumerable<string>> QuickSync(IEnumerable<string> paths, CancellationToken ctoken)
    {
        AssetRecord[] assets = await _assetDatabase.Snapshot(ctoken).ConfigureAwait(false);
        ConcurrentBag<string> pending = new();
        uint success = 0;

        await Parallel.ForEachAsync(paths, ctoken,
            async (path, token) =>
            {
                if (await TrySyncProvisionally(path, assets, token).ConfigureAwait(false))
                    Interlocked.Increment(ref success);
                else
                    pending.Add(path);
            }).ConfigureAwait(false);

        _logger.LogInformation("Provisionally synced {synced} out of {total} files.", success, paths.Count());
        return pending.ToHashSet();
    }

    private async partial Task<bool> TrySyncProvisionally(string path, AssetRecord[] assets, CancellationToken ctoken)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            // Fetch data
            if (await _assetAnalysis.Fetch(path, _options.AssetsPathFull, ctoken).ConfigureAwait(false)
                is not QuickAssetSyncInfo syncInfo)
                throw new IOException($"Unable to read data from {path}"); // Or throw from fetch directly?

            // Attempt sync
            AssetRecord[] matches = assets.Where(rec => rec.ProvisionallyEquals(syncInfo)).ToArray();
            AssetRecord? match = matches.SelectOneNoneOrThrow();

            if (match is null)
                return false;

            // Register and finish
            _assetPresence.Set(syncInfo.LocalPath, match.ItemId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Provisional sync failed for asset at {path}", path);
            return false;
        }
    }
}