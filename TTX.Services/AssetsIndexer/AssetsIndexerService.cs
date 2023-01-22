using Microsoft.Extensions.Logging;
using TTX.Services.AssetInfo;
using TTX.Services.Auxiliary;
using TTX.Services.DbSync;
using TTX.Services.Watcher;

namespace TTX.Services.AssetsIndexer;

/// <summary>
/// Storage class for AssetInfo entities.
/// </summary>
public partial class AssetsIndexerService : IAssetsIndexerService
{
    private readonly IWatcherService _watcher;
    private readonly IDbSyncService _dbsync;
    private readonly IAssetInfoService _assetInfo;

    private readonly IAuxiliaryService _auxiliary;
    private readonly ILogger<AssetsIndexerService> _logger;
    private readonly AssetsIndexerOptions _options;

    public AssetsIndexerService(
        IWatcherService watcher,
        IDbSyncService dbsync,
        IAssetInfoService assetinfo,
        IAuxiliaryService auxiliary,
        ILogger<AssetsIndexerService> logger,
        IOptionsSet options)
    {
        _watcher = watcher;
        _dbsync = dbsync;
        _assetInfo = assetinfo;
        _auxiliary = auxiliary;
        _logger = logger;
        _options = options.ExtractValues<AssetsIndexerOptions>();

        _watcher.StartWatcher(this);
    }

    // Readiness

    public bool IsReady { get; set; } = false;
}