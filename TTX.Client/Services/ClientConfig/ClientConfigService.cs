using Microsoft.Extensions.Logging;
using TTX.Client.Services.LoginGui;
using TTX.Client.Services.MainGui;
using TTX.Client.Services.TagSelectorGui;

namespace TTX.Client.Services.ClientConfig;

internal partial class ClientConfigService : IClientConfigService
{
    private readonly IClientOptions _options;
    private readonly ILogger<ClientConfigService> _logger;

    public ClientConfigService(
        IClientOptions options,
        ILogger<ClientConfigService> logger)
    {
        _options = options;
        _logger = logger;
    }

    // Base Methods

    public void Shutdown()
    {
        // call cancels, etc., then,
        _options.ShutdownAction();
    }

    // Factory Methods

    public ILoginView CreateLoginView()
        => _options.LoginViewFactoryMethod();

    public IMainView CreateMainView()
        => _options.MainViewFactoryMethod();

    public ITagSelectorView CreateTagSelectorView()
        => _options.TagSelectorViewFactoryMethod();
}