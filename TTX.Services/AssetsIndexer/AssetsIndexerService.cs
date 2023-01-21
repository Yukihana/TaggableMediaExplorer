using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Services.AssetInfo;
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

    private readonly ILogger<AssetsIndexerService> _logger;
    private readonly AssetsIndexerOptions _options;

    public AssetsIndexerService(
        IWatcherService watcher,
        IDbSyncService dbsync,
        IAssetInfoService assetinfo,
        ILogger<AssetsIndexerService> logger,
        IOptionsSet options)
    {
        _watcher = watcher;
        _dbsync = dbsync;
        _assetInfo = assetinfo;
        _logger = logger;
        _options = options.ExtractValues<AssetsIndexerOptions>();

        _watcher.StartWatcher(this);
    }

    // Readiness

    private int _isReady = 0;

    public bool IsReady
    {
        get => Interlocked.CompareExchange(ref _isReady, 1, 1) == 1;
        private set
        {
            if (value) Interlocked.CompareExchange(ref _isReady, 1, 0);
            else Interlocked.CompareExchange(ref _isReady, 0, 1);
        }
    }

    // Records

    private readonly SemaphoreSlim _semaphoreRecords = new(1);

    private readonly HashSet<AssetRecord> _records = new();

    private async Task<HashSet<AssetRecord>> Snapshot(CancellationToken token = default)
    {
        try
        {
            await _semaphoreRecords.WaitAsync(token).ConfigureAwait(false);
            return _records.ToHashSet();
        }
        finally { _semaphoreRecords.Release(); }
    }
}