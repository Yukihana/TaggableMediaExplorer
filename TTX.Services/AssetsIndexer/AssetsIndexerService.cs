using System.Threading;
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
    private readonly AssetsIndexerOptions _options;
    private readonly SemaphoreSlim _semaphore;

    public AssetsIndexerService(IWatcherService watcher, IMetadataService metadata, IOptionsSet options)
    {
        _watcher = watcher;
        _metadata = metadata;
        _options = options.ExtractValues<AssetsIndexerOptions>();
        _semaphore = new(1);
    }
}