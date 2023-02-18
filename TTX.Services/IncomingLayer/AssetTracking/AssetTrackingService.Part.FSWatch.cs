using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace TTX.Services.IncomingLayer.AssetTracking;

public partial class AssetTrackingService
{
    private FileSystemWatcher? _watcher = null;

    public partial void StartWatcher(Action onEnqueueAction)
    {
        try { StopWatcher(); }
        finally
        {
            _watcher = new(_options.AssetsPathFull)
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

            _onEnqueueAction = onEnqueueAction;

            _watcher.Created += (sender, eventargs) => EnqueueValidated(eventargs.FullPath);
            _watcher.Renamed += (sender, eventargs) => EnqueueValidated(eventargs.OldFullPath, eventargs.FullPath);
            _watcher.Changed += (sender, eventargs) => EnqueueValidated(eventargs.FullPath);
            _watcher.Deleted += (sender, eventargs) => EnqueueValidated(eventargs.FullPath);
            _watcher.Error += OnError;

            _watcher.EnableRaisingEvents = true;
        }
    }

    public partial void StopWatcher()
    {
        if (_watcher != null)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
        }
    }

    public partial void OnError(object sender, ErrorEventArgs e)
        => _logger.LogError(e.GetException(), "File system watcher has encountered an error.", e);
}