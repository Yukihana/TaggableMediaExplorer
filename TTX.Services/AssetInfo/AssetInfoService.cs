using Microsoft.Extensions.Logging;
using System.Threading;

namespace TTX.Services.AssetInfo;

public partial class AssetInfoService : IAssetInfoService
{
    private readonly AssetInfoOptions _options;
    private readonly ILogger<AssetInfoService> _logger;

    private readonly SemaphoreSlim _semaphoreMetadata;
    private readonly SemaphoreSlim _semaphoreProc;
    private readonly SemaphoreSlim _semaphoreIO;

    public AssetInfoService(ILogger<AssetInfoService> logger, IOptionsSet options)
    {
        _options = options.ExtractValues<AssetInfoOptions>();
        _logger = logger;

        _semaphoreProc = new(_options.HashProcessingConcurrency);
        _semaphoreIO = new(_options.HashIOConcurrency);

        _semaphoreMetadata = new SemaphoreSlim(_options.MetadataConcurrency);
    }
}