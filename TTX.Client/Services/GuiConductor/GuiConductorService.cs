using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;
using TTX.Client.Services.LoginGui;
using TTX.Client.Services.MainGui;

namespace TTX.Client.Services.GuiConductor;

internal class GuiConductorService : IGuiConductorService
{
    private readonly ILoginGuiService _loginGui;
    private readonly IMainGuiService _mainGui;
    private readonly IGuiSyncService _guiSync;
    private readonly IClientConfigService _clientConfig;
    private readonly ILogger<GuiConductorService> _logger;

    public GuiConductorService(
        ILoginGuiService loginGui,
        IMainGuiService mainGui,
        IGuiSyncService guiSync,
        IClientConfigService configService,
        ILogger<GuiConductorService> logger)
    {
        _loginGui = loginGui;
        _mainGui = mainGui;
        _guiSync = guiSync;
        _clientConfig = configService;
        _logger = logger;
    }

    private Task? _activeRoutine = null;

    public void StartRoutine()
    {
        if (_activeRoutine != null)
            _logger.LogError("Client routine is already running.");
        else
            _activeRoutine = Task.Run(async () => await ClientRoutine(_guiSync.CancellationToken).ConfigureAwait(false));
    }

    // Routine components

    private readonly SemaphoreSlim _routineLock = new(1);

    private async Task ClientRoutine(CancellationToken ctoken = default)
    {
        if (_routineLock.CurrentCount == 0)
            return;

        try
        {
            await _routineLock.WaitAsync(ctoken).ConfigureAwait(false);

            if (await _loginGui.ShowModalAsync(ctoken).ConfigureAwait(false))
                await _mainGui.ShowAsync(ctoken).ConfigureAwait(false);
            _guiSync.CancelActiveTasks();
            _guiSync.DispatchPost(_clientConfig.Shutdown, CancellationToken.None);
        }
        finally
        { _routineLock.Release(); }
    }
}