using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.StorageLayer.AssetDatabase;

public interface IAssetDatabaseService
{
    // Standard Api operations

    Task Read(Func<DbSet<AssetRecord>, Task> readAction, CancellationToken ctoken = default);

    Task Write(Func<DbSet<AssetRecord>, Task<bool>> writeAction, CancellationToken ctoken = default);

    Task<AssetRecord[]> Snapshot(CancellationToken ctoken);

    // Addon operations

    Task<AssetRecord> Create(IAssetFullSyncInfo syncInfo, AssetMediaInfo mediaInfo, CancellationToken ctoken = default);

    // Maintenance operations

    Task ScanRepairAnalyse(CancellationToken ctoken);
}