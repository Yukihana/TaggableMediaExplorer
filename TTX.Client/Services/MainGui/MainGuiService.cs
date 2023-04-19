using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;
using TTX.Client.ViewContexts.MainViewContext;

namespace TTX.Client.Services.MainGui;

internal class MainGuiService : IMainGuiService
{
    private readonly ILogger<MainContextLogic> _contextLogger;
    private readonly IGuiSyncService _guiSync;
    private readonly IClientConfigService _clientConfig;
    private readonly ILogger<MainGuiService> _logger;

    public MainGuiService(
        ILogger<MainContextLogic> logicLogger,
        IGuiSyncService guiSync,
        IClientConfigService clientConfig,
        ILogger<MainGuiService> logger)
    {
        _contextLogger = logicLogger;
        _guiSync = guiSync;
        _clientConfig = clientConfig;
        _logger = logger;
    }

    public async Task ShowAsync(CancellationToken ctoken = default)
        => await _guiSync.DispatchActionAsync(Show, ctoken).ConfigureAwait(false);

    private void Show()
    {
        try
        {
            // Create view
            MainContextLogic mainContext = new()
            {
                Logger = _contextLogger,
            };
            IMainView mainView = _clientConfig.CreateMainView();
            mainView.SetViewContext(mainContext);

            // Show and wait for finish
            mainView.ShowView();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed to start MainView");
        }
    }
}