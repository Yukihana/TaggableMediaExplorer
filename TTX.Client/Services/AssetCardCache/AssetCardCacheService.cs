using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;
using TTX.Client.ViewContexts;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.InstancingHelpers;

namespace TTX.Client.Services.AssetCardCache;

internal partial class AssetCardCacheService : IAssetCardCacheService
{
    private readonly IClientConfigService _clientConfig;
    private readonly IGuiSyncService _guiSync;
    private readonly IApiConnectionService _apiConnection;
    private readonly ILogger<AssetCardCacheService> _logger;

    private readonly ConcurrentDictionary<string, AssetCardContext> _cache = new(StringComparer.OrdinalIgnoreCase);

    public AssetCardCacheService(
        IClientConfigService clientConfig,
        IGuiSyncService guiSync,
        IApiConnectionService apiConnection,
        ILogger<AssetCardCacheService> logger)
    {
        _clientConfig = clientConfig;
        _guiSync = guiSync;
        _apiConnection = apiConnection;
        _logger = logger;
    }

    public Dictionary<string, AssetCardContext> Get(string[] idStrings)
    {
        string defaultThumbPath = Path.Combine(_clientConfig.BaseDirectory, "Resources", "imgwait.png");
        Dictionary<string, AssetCardContext> results = new();
        foreach (string idString in idStrings)
        {
            // Get or create in cache
            // Then add it to the results to be returned
            results[idString] = _cache.GetOrAdd(
                key: idString,
                valueFactory: (idStr, thumbPath) => idStr.CreateContext(thumbPath),
                factoryArgument: defaultThumbPath);
        }
        return results;
    }

    public async Task Set(AssetCardState[] cards, CancellationToken ctoken = default)
    {
        foreach (var card in cards)
        {
            ctoken.ThrowIfCancellationRequested();

            // Get the context (previously created by 'Get', or just create one here)
            AssetCardContext target = _cache.GetOrAdd(card.ItemIdString, x => new() { ItemIdString = x });

            // Update the data on the gui thread
            await _guiSync.DispatchActionAsync(
                x => x.card.CopyPropertiesTo(x.target),
                (card, target),
                ctoken
                ).ConfigureAwait(false);
        }
    }
}