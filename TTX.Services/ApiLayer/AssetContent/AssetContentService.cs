using System;
using System.IO;
using TTX.Services.ProcessingLayer.AssetSynchronisation;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.ApiLayer.AssetContent;

public partial class AssetContentService : IAssetContentService
{
    private readonly IAssetPresenceService _assetPresence;
    private readonly AssetContentOptions _options;

    public AssetContentService(
        IAssetPresenceService assetPresence,
        IOptionsSet options)
    {
        _assetPresence = assetPresence;
        _options = options.InitializeServiceOptions<AssetContentOptions>();
    }

    public string? GetPath(string id)
    {
        if (_assetPresence.GetFirst(new Guid(id).ToByteArray()) is string relativePath)
            return Path.Combine(_options.AssetsPathFull, relativePath);
        else return null;
    }
}