using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.ProcessingLayer.AssetAnalysis;

public partial class AssetAnalysisService : IAssetAnalysisService
{
    private readonly AssetAnalysisOptions _options;
    private readonly ILogger<AssetAnalysisService> _logger;

    private readonly SemaphoreSlim _semaphoreMetadata;
    private readonly SemaphoreSlim _semaphoreProc;
    private readonly SemaphoreSlim _semaphoreIO;

    public AssetAnalysisService(
        ILogger<AssetAnalysisService> logger,
        IWorkspaceProfile profile)
    {
        _options = profile.InitializeServiceOptions<AssetAnalysisOptions>();
        _logger = logger;

        _semaphoreProc = new(_options.HashProcessingConcurrency);
        _semaphoreIO = new(_options.HashIOConcurrency);

        _semaphoreMetadata = new SemaphoreSlim(_options.MetadataConcurrency);
    }

    // API

    public partial Task<bool> FileExists(string path, CancellationToken token = default);

    public partial Task<QuickAssetSyncInfo?> Fetch(string path, string relativeTo, CancellationToken token = default);

    public partial Task<FullAssetSyncInfo?> FetchHashed(string path, string relativeTo, CancellationToken token = default);

    private partial Task<T> GetAssetFile<T>(string path, string relativeTo, CancellationToken token = default) where T : AssetSyncInfoBase, IAssetQuickSyncInfo, new();

    // Crumbs

    private static partial long[] GetSpreadIndices(long length, int count);

    private partial Task<byte[]> GetCrumbsAsync(string path, long[] indices, CancellationToken token = default);

    // SHA256

    private partial Task<byte[]> ComputeSHA256Small(string path, CancellationToken token = default);

    private partial Task<byte[]> ComputeSHA256Big(string path, CancellationToken token = default);
}