using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.AssetsIndexer;

public interface IAssetsIndexerService
{
    bool IsReady { get; }

    Task StartIndexing(CancellationToken token = default);

    Task StopIndexing(CancellationToken token = default);

    // FSW

    void OnWatcherEvent(object sender, FileSystemEventArgs e);

    void OnWatcherEvent(object sender, RenamedEventArgs e);
}