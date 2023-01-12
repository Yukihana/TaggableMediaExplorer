using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.Watcher;

public interface IWatcherService
{
    Task<HashSet<string>> GetAllFiles(CancellationToken token = default);
}