using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace TTX.Services.IncomingLayer.AssetTracking;

public partial class AssetTrackingService : IAssetTrackingService
{
    private readonly ILogger<AssetTrackingService> _logger;

    private readonly AssetTrackingOptions _options;

    public AssetTrackingService(ILogger<AssetTrackingService> logger, IOptionsSet options)
    {
        _logger = logger;

        _options = options.InitializeServiceOptions<AssetTrackingOptions>();
    }

    // Queue

    private readonly List<string> _queue = new();
    private readonly ReaderWriterLockSlim _lockQueue = new();

    private partial void EnqueueValidated(params string[] paths);

    private Action? _onEnqueueAction = null;

    public partial string[] Dequeue();

    public partial void ClearPending();

    // Scan

    public partial HashSet<string> GetAllFiles(CancellationToken ctoken = default);

    // FileSystemWatcher

    public partial void StartWatcher(Action onEnqueueAction);

    public partial void StopWatcher();

    public partial void OnError(object sender, ErrorEventArgs e);

    // Validation

    private partial bool ValidatePathByPattern(string path);
}