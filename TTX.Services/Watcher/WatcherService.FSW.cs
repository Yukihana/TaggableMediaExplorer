using Microsoft.Extensions.Logging;
using System.IO;
using TTX.Services.AssetsIndexer;

namespace TTX.Services.Watcher;

public partial class WatcherService
{
    private FileSystemWatcher? _watcher = null;

    public void StartWatcher(IAssetsIndexerService indexer)
    {
        try { StopWatcher(); }
        finally
        {
            _watcher = new()
            {
                IncludeSubdirectories = true,
                Filter = "",
                NotifyFilter
                = NotifyFilters.Attributes
                | NotifyFilters.CreationTime
                | NotifyFilters.DirectoryName
                | NotifyFilters.FileName
                | NotifyFilters.LastAccess
                | NotifyFilters.LastWrite
                | NotifyFilters.Security
                | NotifyFilters.Size,
            };

            _watcher.Created += indexer.OnWatcherEvent;
            _watcher.Renamed += indexer.OnWatcherEvent;
            _watcher.Changed += indexer.OnWatcherEvent;
            _watcher.Deleted += indexer.OnWatcherEvent;
            _watcher.Error += OnError;

            _watcher.EnableRaisingEvents = true;
        }
    }

    public void StopWatcher()
    {
        if (_watcher != null)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
        }
    }

    public void OnError(object sender, ErrorEventArgs e)
        => _logger.LogError(e.GetException(), "File system watcher has encountered an error.", e);
}