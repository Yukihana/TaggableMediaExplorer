using Microsoft.Extensions.Logging;
using TTX.Services.Auxiliary;
using TTX.Services.DbSync;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.AssetsIndexer;

/// <summary>
/// Storage class for AssetInfo entities.
/// </summary>
public partial class AssetsIndexerService : IAssetsIndexerService
{
    private readonly IAssetPresenceService _assetPresence;

    private readonly IAssetTrackingService _assetTracking;

    private readonly IAssetAnalysisService _assetAnalysis;

    private readonly IDbSyncService _dbsync;
    private readonly IAuxiliaryService _auxiliary;

    private readonly ILogger<AssetsIndexerService> _logger;
    private readonly AssetsIndexerOptions _options;

    public AssetsIndexerService(
        IAssetPresenceService assetPresence,
        IAssetTrackingService assetTracking,
        IDbSyncService dbsync,
        IAssetAnalysisService assetinfo,
        IAuxiliaryService auxiliary,
        ILogger<AssetsIndexerService> logger,
        IOptionsSet options)
    {
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
}