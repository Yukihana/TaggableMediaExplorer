using System;
using System.IO;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.ApiLayer.AssetContent;

public partial class AssetContentService : IAssetContentService
{
    private readonly IAssetPresenceService _assetPresence;
    private readonly AssetContentOptions _options;

    public AssetContentService(
        IAssetPresenceService assetPresence,
        IWorkspaceProfile profile,
        IRuntimeConfig config)
    {
        _assetPresence = assetPresence;
        _options = profile.InitializeServiceOptions<AssetContentOptions>(config);
    }

    public string? GetPath(string id)
    {
        if (_assetPresence.GetFirst(new Guid(id).ToByteArray()) is string relativePath)
            return Path.Combine(_options.AssetsPathFull, relativePath);
        else return null;
    }
}