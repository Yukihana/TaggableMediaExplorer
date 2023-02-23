using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TTX.Data.Entities;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.Legacy.Auxiliary;
using TTX.Services.Legacy.DbSync;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.Legacy.AssetsIndexer;

/// <summary>
/// Storage class for AssetInfo entities.
/// </summary>
public partial class AssetsIndexerService : IAssetsIndexerService
{
    private readonly IAssetDatabaseService _assetDatabase;
    private readonly IAssetPresenceService _assetPresence;

    private readonly IAssetTrackingService _assetTracking;

    private readonly IAssetAnalysisService _assetAnalysis;

    private readonly IDbSyncService _dbsync;
    private readonly IAuxiliaryService _auxiliary;

    private readonly ILogger<AssetsIndexerService> _logger;
    private readonly AssetsIndexerOptions _options;

    public AssetsIndexerService(
        IAssetDatabaseService assetDatabase,
        IAssetPresenceService assetPresence,
        IAssetTrackingService assetTracking,
        IDbSyncService dbsync,
        IAssetAnalysisService assetinfo,
        IAuxiliaryService auxiliary,
        ILogger<AssetsIndexerService> logger,
        IOptionsSet options)
    {
        _assetDatabase = assetDatabase;
        _assetPresence = assetPresence;
        _assetTracking = assetTracking;
        _dbsync = dbsync;
        _assetAnalysis = assetinfo;
        _auxiliary = auxiliary;
        _logger = logger;
        _options = options.InitializeServiceOptions<AssetsIndexerOptions>();

        _assetTracking.StartWatcher(IndexPending);
    }

    // Readiness

    public bool IsReady { get; set; } = false;

    // move this to another class
    public TOutput PerformQuery<TInput, TOutput>(TInput input, Func<TInput, IEnumerable<AssetRecord>, TOutput> func)
    {
        return func(input, _assetDatabase.Snapshot().Where(rec => _assetPresence.GetFirst(rec.ItemId) is not null));
    }
}