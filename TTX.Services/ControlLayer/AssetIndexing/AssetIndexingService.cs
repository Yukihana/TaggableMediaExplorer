using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.ProcessingLayer.AssetSynchronisation;

namespace TTX.Services.ControlLayer.AssetIndexing;

// Use interlocked int, ManualResetEvent and SemaphoreSlim(procCount)
// Set signal, Task.WhenAll, execute special task, change interlocked int value, reset signal
// when new tasks are created, they get the interlocked int value
// if after MRE.Wait the int value is still the same, continue, otherwise drop task

public partial class AssetIndexingService : IAssetIndexingService
{
    // Services

    private readonly IAssetSynchronisationService _assetSynchronisation;
    private readonly IAssetTrackingService _assetTracking;
    private readonly ILogger<AssetIndexingService> _logger;
    private readonly AssetIndexingOptions _options;

    // Control

    private readonly SemaphoreSlim _semaphoreControl = new(1);

    // Tasks

    private CancellationTokenSource _cts = new();
    private List<Task> _tasks = new();
    private ReaderWriterLockSlim _lockTasks = new();

    // Processing

    private readonly int _concurrency;
    private readonly SemaphoreSlim _semaphoreSync;

    // Gate

    private int _gateId = 0;
    private readonly AsyncManualResetEvent _gate = new(false);

    // Init

    public AssetIndexingService(
        IAssetSynchronisationService assetSynchronisation,
        IAssetTrackingService assetTracking,
        ILogger<AssetIndexingService> logger,
        IOptionsSet options)
    {
        _assetSynchronisation = assetSynchronisation;
        _assetTracking = assetTracking;
        _logger = logger;
        _options = options.InitializeServiceOptions<AssetIndexingOptions>();

        _concurrency = Environment.ProcessorCount;
        _semaphoreSync = new(_concurrency);

        // Ensure new tasks cancel automatically since indexing hasn't been started yet
        _cts.Cancel();
    }

    // Control Api

    public partial Task StartIndexing(CancellationToken ctoken = default);

    public partial Task StopIndexing(CancellationToken ctoken = default);

    // Queue Access

    private partial Task[] GetActiveTasks(CancellationToken ctoken = default);

    private partial void QueueSynchronisationTask(string path, int sessionId);

    private partial Task WaitForSemaphoreClearance(CancellationToken ctoken = default);

    // Private Uncat

    private partial Task Reload(CancellationToken ctoken = default);
}