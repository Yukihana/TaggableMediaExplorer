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
    private readonly AssetsIndexerOptions _options;

    public AssetsIndexerService(IWatcherService watcher, IMetadataService metadata, IDbSyncService dbsync, IOptionsSet options)
    {
        _watcher = watcher;
        _metadata = metadata;
        _dbsync = dbsync;
        _options = options.ExtractValues<AssetsIndexerOptions>();
    }
}