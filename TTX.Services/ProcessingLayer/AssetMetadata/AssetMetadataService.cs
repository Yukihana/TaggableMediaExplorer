using Microsoft.Extensions.Logging;

namespace TTX.Services.ProcessingLayer.AssetMetadata;

public partial class AssetMetadataService : IAssetMetadataService
{
    private readonly ILogger<AssetMetadataService> _logger;
    private readonly AssetMetadataOptions _options;

    public AssetMetadataService(
        ILogger<AssetMetadataService> logger,
        IWorkspaceProfile profile)
    {
        _logger = logger;
        _options = profile.InitializeServiceOptions<AssetMetadataOptions>();
    }
}