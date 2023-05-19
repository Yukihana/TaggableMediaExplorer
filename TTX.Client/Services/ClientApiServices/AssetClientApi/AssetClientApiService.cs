using System;
using System.Collections.Concurrent;
using System.IO;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.ClientSession;
using TTX.Client.ViewContexts;

namespace TTX.Client.Services.ClientApiServices.AssetClientApi;

internal class AssetClientApiService : IAssetClientApiService
{
    private readonly IClientConfigService _clientConfig;
    private readonly IClientSessionService _clientSession;

    private readonly ConcurrentDictionary<string, AssetCardContext> _assetCards = new(StringComparer.OrdinalIgnoreCase);
    private readonly string _defaultPreviewPath = string.Empty;

    public AssetClientApiService(
        IClientConfigService clientConfig,
        IClientSessionService clientSession)
    {
        _clientConfig = clientConfig;
        _clientSession = clientSession;

        _defaultPreviewPath = Path.Combine(_clientConfig.BaseDirectory, "Resources", "imgwait.png");
    }

    // Create or Get, Not update

    public AssetCardContext GetAssetCard(string id)
    {
        return _assetCards.GetOrAdd(
            key: id,
            valueFactory: CreateDefault);
    }

    public AssetCardContext CreateDefault(string id)
    {
        return new()
        {
            ItemId = id,
            ThumbPath = _defaultPreviewPath,
        };
    }
}