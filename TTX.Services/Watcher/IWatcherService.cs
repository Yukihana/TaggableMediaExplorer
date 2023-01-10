using System.Collections.Generic;
using System.Threading.Tasks;
using TTX.Data.Messages;

namespace TTX.Services.Watcher;

public interface IWatcherService
{
    Task<List<AssetFile>> GetFiles();
}