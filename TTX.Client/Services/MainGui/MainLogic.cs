using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;
using TTX.Client.Services.PreviewLoader;

namespace TTX.Client.Services.MainGui;

public partial class MainLogic : ObservableObject
{
    private readonly IGuiSyncService _guiSync;
    private readonly IClientConfigService _clientConfig;
    private readonly IApiConnectionService _apiConnection;
    private readonly IPreviewLoaderService _previewLoader;

    private readonly SemaphoreSlim _semaphoreResultsDispatch = new(1);
    public ILogger<MainLogic>? Logger { get; init; } = null;

    public MainLogic() : base()
    {
        _guiSync = ClientContextHost.GetService<IGuiSyncService>();
        _clientConfig = ClientContextHost.GetService<IClientConfigService>();
        _apiConnection = ClientContextHost.GetService<IApiConnectionService>();
        _previewLoader = ClientContextHost.GetService<IPreviewLoaderService>();
    }

    [ObservableProperty]
    private MainData _dataModel = new();

    public void GuiLoaded()
        => _ = Task.Run(async () => await SearchNew(string.Empty, _guiSync.CancellationToken).ConfigureAwait(false));
}