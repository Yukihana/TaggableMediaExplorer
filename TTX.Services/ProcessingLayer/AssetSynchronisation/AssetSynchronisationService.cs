using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData.Entities;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;
using TTX.Services.StorageLayer.AssetPreview;
using TTX.Services.StorageLayer.MediaCodec;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService : IAssetSynchronisationService
{
    private readonly IAssetDatabaseService _assetDatabase;
    private readonly IAssetPresenceService _assetPresence;
    private readonly IAssetAnalysisService _assetAnalysis;
    private readonly IAssetPreviewService _assetPreview;
    private readonly IMediaCodecService _mediaCodec;

    private readonly ILogger<AssetSynchronisationService> _logger;
    private readonly AssetSynchronisationOptions _options;

    private readonly SemaphoreSlim _semaphore = new(1);

    public AssetSynchronisationService(
        IAssetDatabaseService assetDatabase,
        IAssetPresenceService assetPresence,
        IAssetAnalysisService assetAnalysis,
        IAssetPreviewService assetPreview,
        IMediaCodecService mediaCodec,

        ILogger<AssetSynchronisationService> logger,
        IWorkspaceProfile profile,
        IRuntimeConfig config)
    {
        _assetDatabase = assetDatabase;
        _assetPresence = assetPresence;
        _assetAnalysis = assetAnalysis;
        _assetPreview = assetPreview;
        _mediaCodec = mediaCodec;
        _logger = logger;
        _options = profile.InitializeServiceOptions<AssetSynchronisationOptions>(config);
    }

    // Reload

    public partial Task ResetRepair(CancellationToken ctoken = default);

    // Quick Sync

    public partial Task<IEnumerable<string>> QuickSync(IEnumerable<string> paths, CancellationToken ctoken = default);

    private partial Task<bool> TrySyncProvisionally(string path, AssetRecord[] assets, CancellationToken ctoken = default);

    // Full Sync

    public partial Task<bool> FullSync(string path, bool isReloadSync = false, CancellationToken ctoken = default);

    private partial Task<bool> AttemptFullSync(string path, CancellationToken ctoken = default);

    // Post Sync

    private partial Task OnSyncSuccess(AssetRecord asset, CancellationToken ctoken = default);
}