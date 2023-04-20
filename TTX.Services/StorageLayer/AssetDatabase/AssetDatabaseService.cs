using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService : IAssetDatabaseService
{
    private readonly IDbContextFactory<AssetsContext> _dbContextFactory;

    private readonly ILogger<AssetDatabaseService> _logger;
    private readonly AssetDatabaseOptions _options;

    public AssetDatabaseService(
        IDbContextFactory<AssetsContext> dbContextFactory,
        ILogger<AssetDatabaseService> logger,
        IWorkspaceProfile profile)
    {
        _dbContextFactory = dbContextFactory;

        _logger = logger;
        _options = profile.InitializeServiceOptions<AssetDatabaseOptions>();
    }

    // Standard Api operations

    public partial Task Read(Func<DbSet<AssetRecord>, Task> readAction, CancellationToken ctoken = default);

    public partial Task Write(Func<DbSet<AssetRecord>, Task<bool>> writeAction, CancellationToken ctoken = default);

    public partial Task<AssetRecord[]> Snapshot(CancellationToken ctoken = default);

    // Addon operations

    public partial Task<AssetRecord> Create(IAssetFullSyncInfo syncInfo, AssetMediaInfo mediaInfo, CancellationToken ctoken = default);

    // Repair

    public partial Task ScanRepairAnalyse(CancellationToken ctoken = default);

    private partial bool TrackedAnalysis(DbSet<AssetRecord> assets, CancellationToken ctoken = default);

    private partial bool FixItemIdConflicts(IEnumerable<AssetRecord> assets, CancellationToken ctoken = default);

    private partial void UntrackedAnalysis(IEnumerable<AssetRecord> assets, CancellationToken ctoken = default);

    private partial void ScanForIntegrityConflicts(IEnumerable<AssetRecord> assets, CancellationToken ctoken = default);
}