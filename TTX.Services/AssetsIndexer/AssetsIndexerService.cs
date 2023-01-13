using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TTX.Services.DbSync;
using TTX.Services.Metadata;
using TTX.Services.Watcher;

namespace TTX.Services.AssetsIndexer;

/// <summary>
/// Storage class for AssetInfo entities.
/// </summary>
public partial class AssetsIndexerService : IAssetsIndexerService
{
    private readonly IWatcherService _watcher;
    private readonly IMetadataService _metadata;
    private readonly IDbSyncService _dbsync;

    private readonly ILogger<AssetsIndexerService> _logger;
    private readonly AssetsIndexerOptions _options;

    public AssetsIndexerService(
        IWatcherService watcher,
        IMetadataService metadata,
        IDbSyncService dbsync,
        ILogger<AssetsIndexerService> logger,
        IOptionsSet options)
    {
        _watcher = watcher;
        _metadata = metadata;
        _dbsync = dbsync;
        _logger = logger;
        _options = options.ExtractValues<AssetsIndexerOptions>();
    }

    // Readiness

    private int _isReady = 0;
    public bool IsReady
    {
        get { return (Interlocked.CompareExchange(ref _isReady, 1, 1) == 1); }
        private set
        {
            if (value) Interlocked.CompareExchange(ref _isReady, 1, 0);
            else Interlocked.CompareExchange(ref _isReady, 0, 1);
        }
    }
}