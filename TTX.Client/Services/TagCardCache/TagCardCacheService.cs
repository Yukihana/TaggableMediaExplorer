using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;
using TTX.Client.ViewContexts;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.InstancingHelpers;

namespace TTX.Client.Services.TagCardCache;

internal partial class TagCardCacheService : ITagCardCacheService
{
    private readonly IClientConfigService _clientConfig;
    private readonly IGuiSyncService _guiSync;
    private readonly IApiConnectionService _apiConnection;
    private readonly ILogger<TagCardCacheService> _logger;

    private readonly ConcurrentDictionary<string, TagCardContext> _cache = new(StringComparer.OrdinalIgnoreCase);

    public TagCardCacheService(
        IClientConfigService clientConfig,
        IGuiSyncService guiSync,
        IApiConnectionService apiConnection,
        ILogger<TagCardCacheService> logger)
    {
        _clientConfig = clientConfig;
        _guiSync = guiSync;
        _apiConnection = apiConnection;
        _logger = logger;
    }

    public Dictionary<string, TagCardContext> Get(string[] tagIds)
    {
        string defaultTagColor = "#CCCCCC";
        Dictionary<string, TagCardContext> results = new();
        foreach (string tagId in tagIds)
        {
            // Get or create in cache
            // Then add it to the results to be returned
            results[tagId] = _cache.GetOrAdd(
                key: tagId,
                valueFactory: (id, color) => id.CreateTagCardContext(color),
                factoryArgument: defaultTagColor);
        }
        return results;
    }

    public async Task Set(TagCardState[] tags, CancellationToken ctoken = default)
    {
        foreach (var tag in tags)
        {
            ctoken.ThrowIfCancellationRequested();

            // Get the context (previously created by 'Get', or just create one here)
            TagCardContext target = _cache.GetOrAdd(tag.TagId, x => new() { TagId = x });

            // Update the data on the gui thread
            await _guiSync.DispatchActionAsync(
                x => x.tag.CopyPropertiesTo(x.target),
                (tag, target),
                ctoken
                ).ConfigureAwait(false);
        }
    }
}