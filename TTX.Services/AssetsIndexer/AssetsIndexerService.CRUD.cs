using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    // Create

    private async Task CreateRecord(AssetFile file, CancellationToken token = default)
    {
        // Process
        AssetRecord rec = new();

        // Push to DB

        // Update In-Memory
        try
        {
            _lockRecords.EnterWriteLock();
            _records.Add(rec);
        }
        finally { _lockRecords.ExitWriteLock(); }
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

    private async Task UpdateRecord(byte[] guid, Action<AssetRecord> action, CancellationToken token = default)
    {
    }

    // Delete

    private async Task DeleteRecord(byte[] guid, CancellationToken token = default)
    {
    }
}