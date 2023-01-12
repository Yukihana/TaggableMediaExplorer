using System.Collections.Generic;
using System.Threading.Tasks;
using TTX.Data.Messages;

namespace TTX.Services.Watcher;

public partial class WatcherService : IWatcherService
{
    private readonly WatcherOptions _options;

    public WatcherService(IOptionsSet options)
    {
        _options = options.ExtractValues<WatcherOptions>();
    }
}