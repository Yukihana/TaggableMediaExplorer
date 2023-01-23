using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Extensions;
using TTX.Data.Models;
using TTX.Library.Helpers;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    // Create

    private async Task<bool> CreateRecord(AssetFile file, string localPath, CancellationToken token = default)
    {
        // Process
        AssetRecord rec = file.GenerateAssetRecord(localPath);

        // Push to DB
        if (!await _dbsync.AddRecord(rec, token).ConfigureAwait(false))
            return false;

        // Update In-Memory
        SafeAddToRecords(rec);
        return true;
    }

    // Reload

    private async Task ReloadRecords(CancellationToken token = default)
    {
        Stopwatch timer = Stopwatch.StartNew();

        List<AssetRecord> loaded = await _dbsync.LoadAssets(token).ConfigureAwait(false);
        try
        {
            _lockRecords.EnterWriteLock();
            _records.Clear();
            for (int i = 0; i < loaded.Count; i++)
                _records.Add(loaded[i]);
        }
        finally { _lockRecords.ExitWriteLock(); }

        timer.Stop();
        _logger.LogInformation("Records loaded in {elapsed} ms.", timer.Elapsed);
    }

    // Update

    private async Task<bool> UpdateRecord(AssetRecord rec, Action<AssetRecord> action, CancellationToken token = default)
    {
        // Prepare
        byte[] guid = rec.SafeRead(x => x.GUID, rec.Lock);

        // Update Storage DB
        if (!await _dbsync.UpdateRecord(guid, action, token).ConfigureAwait(false))
            return false;

        // Update In-Memory DB
        rec.SafeWrite(action, rec.Lock);
        return true;
    }

    // Delete

    private async Task DeleteRecord(byte[] guid, CancellationToken token = default)
    {
    }
}