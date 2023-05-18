using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.GuiSync;
using TTX.Client.ViewContexts.TagSelectorViewContext;

namespace TTX.Client.Services.TagSelectorGui;

internal class TagSelectorGuiService : ITagSelectorGuiService
{
    private readonly ILogger<TagSelectorContextLogic> _logicLogger;
    private readonly IGuiSyncService _guiSync;

    private readonly IClientConfigService _clientConfig;
    private readonly ILogger<TagSelectorGuiService> _logger;

    public TagSelectorGuiService(
        ILogger<TagSelectorContextLogic> logicLogger,
        IGuiSyncService guiSync,
        IClientConfigService clientConfig,
        ILogger<TagSelectorGuiService> logger)
    {
        _logicLogger = logicLogger;
        _guiSync = guiSync;
        _clientConfig = clientConfig;
        _logger = logger;
    }

    public async Task<string?> ShowModalAsync(CancellationToken ctoken = default)
        => await _guiSync.DispatchFuncAsync(ShowModal, ctoken).ConfigureAwait(false);

    private string? ShowModal()
    {
        // Setup the context
        TagSelectorContextLogic logic = new()
        {
            Logger = _logicLogger,
        };

        // Create the view and attach the context
        ITagSelectorView view = _clientConfig.CreateTagSelectorView();
        view.SetViewContext(logic);

        // Show, wait and return
        return view.ShowModal();
    }
}