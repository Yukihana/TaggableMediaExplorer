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

    void OnCreated(object sender, FileSystemEventArgs e);

    void OnDeleted(object sender, FileSystemEventArgs e);

    void OnRenamed(object sender, RenamedEventArgs e);

    void OnChanged(object sender, FileSystemEventArgs e);
}