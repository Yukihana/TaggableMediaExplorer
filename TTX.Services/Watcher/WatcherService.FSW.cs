using System.IO;
using System;

namespace TTX.Services.Watcher;

public partial class WatcherService
{
    private FileSystemWatcher? _watcher = null;

    public void StartWatcher()
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

            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;
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

    private void OnError(object sender, ErrorEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        throw new NotImplementedException();
    }
}