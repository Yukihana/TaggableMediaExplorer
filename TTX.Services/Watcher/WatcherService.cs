using Microsoft.Extensions.Logging;

namespace TTX.Services.Watcher;

public partial class WatcherService : IWatcherService
{
    private readonly ILogger<WatcherService> _logger;

    private readonly WatcherOptions _options;

    public WatcherService(ILogger<WatcherService> logger, IOptionsSet options)
    {
        _logger = logger;

        _options = options.InitializeServiceOptions<WatcherOptions>();
    }
}