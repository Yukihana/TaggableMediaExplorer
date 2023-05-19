using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.ClientApiServices.AssetClientApi;
using TTX.Client.Services.ClientApiServices.TagClientApi;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;
using TTX.Client.Services.PreviewLoader;
using TTX.Client.Services.TagSelectorGui;
using TTX.Library.Helpers.EnumerableHelpers;
using TTX.Library.InstancingHelpers;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic : ObservableObject
{
    // Services

    private readonly IGuiSyncService _guiSync;
    private readonly IClientConfigService _clientConfig;
    private readonly IApiConnectionService _apiConnection;
    private readonly IPreviewLoaderService _previewLoader;
    private readonly IAssetClientApiService _assetClientApi;
    private readonly ITagClientApiService _tagClientApi;
    private readonly ITagSelectorGuiService _tagSelectorGui;

    // Addons

    public ILogger<BrowserContextLogic>? Logger { get; set; } = null;

    // Data and Subview contexts

    [ObservableProperty]
    private BrowserContextData _contextData = new();

    // Init

    public BrowserContextLogic()
    {
        _guiSync = ClientContextHost.GetService<IGuiSyncService>();
        _clientConfig = ClientContextHost.GetService<IClientConfigService>();
        _apiConnection = ClientContextHost.GetService<IApiConnectionService>();
        _previewLoader = ClientContextHost.GetService<IPreviewLoaderService>();
        _assetClientApi = ClientContextHost.GetService<IAssetClientApiService>();
        _tagClientApi = ClientContextHost.GetService<ITagClientApiService>();
        _tagSelectorGui = ClientContextHost.GetService<ITagSelectorGuiService>();
    }

    // Gui events

    public void GuiLoaded()
        => _ = Task.Run(async () => await SearchNew(string.Empty, _guiSync.CancellationToken).ConfigureAwait(false));

    // Updates

    private async Task UpdateContexts<TState, TContext>(List<(TState State, TContext Context)> resultPairs, int batchSize, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        foreach (var batch in resultPairs.InBatches(batchSize))
        {
            // dispatch updates for each batch
            await _guiSync.DispatchActionAsync(pairs =>
            {
                foreach (var (state, context) in pairs)
                {
                    try { state.CopyPropertiesTo(context); }
                    catch (Exception ex) { Logger?.LogError(ex, "Failed to update the context from: {state}", state); }
                }
            }, batch, ctoken).ConfigureAwait(false);
        }
    }
}