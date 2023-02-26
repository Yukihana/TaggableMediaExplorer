using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;

namespace TTX.Services.IncomingLayer.AssetTracking;

public sealed partial class AssetTrackingService : IAssetTrackingService, IDisposable
{
    private readonly ILogger<AssetTrackingService> _logger;
    private readonly AssetTrackingOptions _options;

    private readonly FileSystemWatcher _watcher;
    private Action<string, DateTime>? _watcherAction = null;

    public AssetTrackingService(ILogger<AssetTrackingService> logger, IOptionsSet options)
    {
        _logger = logger;
        _options = options.InitializeServiceOptions<AssetTrackingOptions>();

        _watcher = new(_options.AssetsPathFull);
        SetupWatcher();
    }

    // Watcher control

    public partial void StartWatcher(Action<string, DateTime> watcherAction);

    public partial void StopWatcher();

    // Watcher actions

    private partial void SetupWatcher();

    private partial void EnqueueValidated(params string[] paths);

    public partial void OnError(object sender, ErrorEventArgs e);

    // Scan and Misc

    public partial string[] GetAllFiles(CancellationToken ctoken = default);

    private partial bool ValidatePathByPattern(string path);

    // Cleanup

    public partial void Dispose();
}