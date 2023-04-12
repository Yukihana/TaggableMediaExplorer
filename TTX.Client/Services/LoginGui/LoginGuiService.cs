using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;

namespace TTX.Client.Services.LoginGui;

internal class LoginGuiService : ILoginGuiService
{
    private readonly IGuiSyncService _guiSync;
    private readonly IClientConfigService _clientConfig;
    private readonly ILogger<LoginGuiService> _logger;

    public LoginGuiService(
        IGuiSyncService guiSync,
        IClientConfigService clientConfig,
        ILogger<LoginGuiService> logger)
    {
        _guiSync = guiSync;
        _clientConfig = clientConfig;
        _logger = logger;
    }

    public async Task<bool> ShowModalAsync(CancellationToken ctoken = default)
        => await _guiSync.DispatchFuncAsync(ShowModal, ctoken).ConfigureAwait(false);

    private bool ShowModal()
    {
        ILoginView loginView = _clientConfig.CreateLoginView();
        return loginView.ShowModal() == 1;
    }
}