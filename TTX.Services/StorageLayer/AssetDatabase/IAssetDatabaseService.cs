using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.StorageLayer.AssetDatabase;

// Combined responsibilities.
// Handles DbSync for Assets
// Also caches the Db in memory
// DbSync(Assets) + AssetsStorage
// Does not return nor expose AssetRecords in any way, even in transform actions

public interface IAssetDatabaseService
{
    // Controls

    Task<bool> Reload(CancellationToken ctoken = default);

    int CacheCount();

    // API : Read

    // API : Write

    Task<byte[]?> Create(FullAssetSyncInfo info, CancellationToken ctoken = default);

    Task<bool> Update(byte[] itemId, Action<AssetRecord> updateAction, CancellationToken ctoken = default);

    Task<bool> Delete(byte[] itemId, CancellationToken ctoken = default);

    // Temporary
    List<AssetRecord> Snapshot();
}