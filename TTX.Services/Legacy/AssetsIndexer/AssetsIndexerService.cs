using Microsoft.Extensions.Logging;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.ProcessingLayer.AssetSynchronisation;

namespace TTX.Services.Legacy.AssetsIndexer;

/// <summary>
/// Storage class for AssetInfo entities.
/// </summary>
public partial class AssetsIndexerService : IAssetsIndexerService
{
    private readonly IAssetTrackingService _assetTracking;
    private readonly IAssetSynchronisationService _assetSynchronisation;

    private readonly ILogger<AssetsIndexerService> _logger;
    private readonly AssetsIndexerOptions _options;

    public AssetsIndexerService(
        IAssetTrackingService assetTracking,
        IAssetSynchronisationService assetSynchronisation,

        ILogger<AssetsIndexerService> logger,
        IOptionsSet options)
    {
        _assetTracking = assetTracking;
        _assetSynchronisation = assetSynchronisation;

        _logger = logger;
        _options = options.InitializeServiceOptions<AssetsIndexerOptions>();

        _assetTracking.StartWatcher(IndexPending);
    }

    // Readiness

    public bool IsReady { get; set; } = false;
}