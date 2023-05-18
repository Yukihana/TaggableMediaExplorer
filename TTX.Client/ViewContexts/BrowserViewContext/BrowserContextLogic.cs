using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.AssetCardCache;
using TTX.Client.Services.ClientApiServices.TagClientApi;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;
using TTX.Client.Services.PreviewLoader;
using TTX.Client.Services.TagCardCache;
using TTX.Client.Services.TagSelectorGui;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic : ObservableObject
{
    // Services

    private readonly IGuiSyncService _guiSync;
    private readonly IClientConfigService _clientConfig;
    private readonly IApiConnectionService _apiConnection;
    private readonly IAssetCardCacheService _assetCardCache;
    private readonly IPreviewLoaderService _previewLoader;
    private readonly ITagCardCacheService _tagCardCache;
    private readonly ITagClientApiService _tagClientApi;
    private readonly ITagSelectorGuiService _tagSelectorGui;

    // Addons

    public ILogger<BrowserContextLogic>? Logger { get; set; } = null;

    // Data and Subview contexts

    [ObservableProperty]
    private BrowserContextData _contextData = new();

    // Internal

    private readonly SemaphoreSlim _semaphoreResultsDispatch = new(1);

    // Init

    public BrowserContextLogic()
    {
        _guiSync = ClientContextHost.GetService<IGuiSyncService>();
        _clientConfig = ClientContextHost.GetService<IClientConfigService>();
        _apiConnection = ClientContextHost.GetService<IApiConnectionService>();
        _assetCardCache = ClientContextHost.GetService<IAssetCardCacheService>();
        _previewLoader = ClientContextHost.GetService<IPreviewLoaderService>();
        _tagCardCache = ClientContextHost.GetService<ITagCardCacheService>();
        _tagClientApi = ClientContextHost.GetService<ITagClientApiService>();
        _tagSelectorGui = ClientContextHost.GetService<ITagSelectorGuiService>();
    }

    // Gui events

    public void GuiLoaded()
        => _ = Task.Run(async () => await SearchNew(string.Empty, _guiSync.CancellationToken).ConfigureAwait(false));

    // Api
}