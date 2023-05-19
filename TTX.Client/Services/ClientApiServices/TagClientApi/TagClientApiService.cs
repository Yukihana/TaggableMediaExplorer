using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using TTX.Client.Services.ClientSession;
using TTX.Client.Services.GuiSync;
using TTX.Client.ViewContexts;

namespace TTX.Client.Services.ClientApiServices.TagClientApi;

internal partial class TagClientApiService : ITagClientApiService
{
    private readonly IGuiSyncService _guiSync;
    private readonly IClientSessionService _clientSession;
    private readonly ILogger<TagClientApiService> _logger;

    private readonly ConcurrentDictionary<string, TagCardContext> _tagCards = new(StringComparer.OrdinalIgnoreCase);
    private readonly string _defaultTagColor = string.Empty;

    public TagClientApiService(
        IGuiSyncService guiSync,
        IClientSessionService clientSession,
        ILogger<TagClientApiService> logger)
    {
        _guiSync = guiSync;
        _clientSession = clientSession;
        _logger = logger;

        _defaultTagColor = "#888888"; // TODO get this parameter from client config (decide on local disabled tag color)
    }
}