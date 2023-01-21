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

    private async Task CreateRecord(HashedAssetFile file, CancellationToken token = default)
    {
        // Process
        AssetRecord rec = new();

        // Push to DB

        // Update In-Memory
        try
        {
            await _semaphoreRecords.WaitAsync(token).ConfigureAwait(false);
            _records.Add(rec);
        }
        finally { _semaphoreRecords.Release(); }
    }

    // Reload

    private async Task ReloadRecords(CancellationToken token = default)
    {
        Stopwatch timer = Stopwatch.StartNew();

        List<AssetRecord> loaded = await _dbsync.LoadAssets(token).ConfigureAwait(false);
        try
        {
            await _semaphoreRecords.WaitAsync(token).ConfigureAwait(false);
            _records.Clear();
            for (int i = 0; i < loaded.Count; i++)
                _records.Add(loaded[i]);
        }
        finally { _semaphoreRecords.Release(); }

        timer.Stop();
        _logger.LogInformation("Records loaded in {elapsed} ms.", timer.Elapsed);
    }

    // Update

    private async Task UpdateRecord(byte[] guid, Action<AssetRecord> action, CancellationToken token = default)
    {
        // Process
        AssetRecord rec = new();

        // Push to DB

        // Update In-Memory
        try
        {
            await _semaphoreRecords.WaitAsync(token).ConfigureAwait(false);
            _records.Add(rec);
        }
        finally { _semaphoreRecords.Release(); }
    }

    // Delete

    private async Task DeleteRecord(byte[] guid, CancellationToken token = default)
    {
        // Push to DB (Find record by GUID, Delete)

        // Update In-Memory (Find record by GUID, Delete)
        try
        {
            await _semaphoreRecords.WaitAsync(token).ConfigureAwait(false);
        }
        finally { _semaphoreRecords.Release(); }
    }
}