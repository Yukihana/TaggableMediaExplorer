using Microsoft.Extensions.Logging;
using System.Threading;

namespace TTX.Services.ProcessingLayer.AssetAnalysis;

public partial class AssetAnalysisService : IAssetAnalysisService
{
    private readonly AssetAnalysisOptions _options;
    private readonly ILogger<AssetAnalysisService> _logger;

    private readonly SemaphoreSlim _semaphoreMetadata;
    private readonly SemaphoreSlim _semaphoreProc;
    private readonly SemaphoreSlim _semaphoreIO;

    public AssetAnalysisService(ILogger<AssetAnalysisService> logger, IOptionsSet options)
    {
        _options = options.InitializeServiceOptions<AssetAnalysisOptions>();
        _logger = logger;

        _semaphoreProc = new(_options.HashProcessingConcurrency);
        _semaphoreIO = new(_options.HashIOConcurrency);

        _semaphoreMetadata = new SemaphoreSlim(_options.MetadataConcurrency);
    }
}