using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService : IAssetSynchronisationService
{
    private readonly IAssetDatabaseService _assetDatabase;
    private readonly IAssetPresenceService _assetPresence;
    private readonly IAssetAnalysisService _assetAnalysis;

    private readonly ILogger<AssetSynchronisationService> _logger;
    private readonly AssetSynchronisationOptions _options;

    private readonly SemaphoreSlim _semaphore = new(1);

    public AssetSynchronisationService(
        IAssetDatabaseService assetDatabase,
        IAssetPresenceService assetPresence,
        IAssetAnalysisService assetAnalysis,

        ILogger<AssetSynchronisationService> logger,
        IOptionsSet options)
    {
        _assetDatabase = assetDatabase;
        _assetPresence = assetPresence;
        _assetAnalysis = assetAnalysis;
        _logger = logger;
        _options = options.InitializeServiceOptions<AssetSynchronisationOptions>();
    }

    // Reload

    public partial Task ResetRepair(CancellationToken ctoken = default);

    // Quick Sync

    public partial Task<IEnumerable<string>> QuickSync(IEnumerable<string> paths, CancellationToken ctoken = default);

    private partial Task<bool> TrySyncProvisionally(string path, AssetRecord[] assets, CancellationToken ctoken = default);

    // Full Sync

    public partial Task<bool> FullSync(string path, bool isReloadSync = false, CancellationToken ctoken = default);

    private partial Task<bool> AttemptFullSync(string path, CancellationToken ctoken = default);
}