using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace TTX.Services.IncomingLayer.AssetTracking;

public partial class AssetTrackingService
{
    private partial void SetupWatcher()
    {
        _watcher.IncludeSubdirectories = true;
        _watcher.Filter = "";
        _watcher.NotifyFilter
            = NotifyFilters.Attributes
            | NotifyFilters.CreationTime
            | NotifyFilters.DirectoryName
            | NotifyFilters.FileName
            | NotifyFilters.LastAccess
            | NotifyFilters.LastWrite
            | NotifyFilters.Security
            | NotifyFilters.Size;

        _watcher.Created += (sender, eventargs) => EnqueueValidated(eventargs.FullPath);
        _watcher.Renamed += (sender, eventargs) => EnqueueValidated(eventargs.OldFullPath, eventargs.FullPath);
        _watcher.Changed += (sender, eventargs) => EnqueueValidated(eventargs.FullPath);
        _watcher.Deleted += (sender, eventargs) => EnqueueValidated(eventargs.FullPath);
        _watcher.Error += OnError;
    }

    public partial void StartWatcher(Action<string, DateTime> watcherAction)
    {
        try
        {
            _watcher.EnableRaisingEvents = false;
            _watcherAction = watcherAction;
            _watcher.EnableRaisingEvents = true;
            _logger.LogInformation("Started tracking events in path: ", _watcher.Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed trying to start the tracker.");
        }
    }

    public partial void StopWatcher()
    {
        _watcher.EnableRaisingEvents = false;
        _watcherAction = null;
        _logger.LogInformation("Stopped tracking events in path: ", _watcher.Path);
    }

    public partial void Dispose()
    {
        StopWatcher();
        _watcher.Dispose();
    }
}