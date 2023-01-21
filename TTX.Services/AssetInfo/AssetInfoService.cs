using Microsoft.Extensions.Logging;
using System.Threading;

namespace TTX.Services.AssetInfo;

public partial class AssetInfoService : IAssetInfoService
{
    private readonly AssetInfoOptions _options;
    private readonly ILogger<AssetInfoService> _logger;

    private readonly SemaphoreSlim _semaphore;
    private readonly SemaphoreSlim _semaphoreProc;
    private readonly SemaphoreSlim _semaphoreIO;
    private readonly SemaphoreSlim _semaphoreCrumbs = new(1);

    public AssetInfoService(ILogger<AssetInfoService> logger, IOptionsSet options)
    {
        _options = options.ExtractValues<AssetInfoOptions>();
        _logger = logger;

        _semaphoreProc = new(_options.HashProcessingConcurrency);
        _semaphoreIO = new(_options.HashIOConcurrency);

        _semaphore = new SemaphoreSlim(_options.MetadataConcurrency);
    }
}