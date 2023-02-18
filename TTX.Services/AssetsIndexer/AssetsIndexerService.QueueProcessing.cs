using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Library.FileSystemHelpers;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    // Processing Task

    private CancellationTokenSource _cts = new();
    private Task? _processingTask = null;
    private volatile bool _queueProcessingEnabled = false;
    private readonly SemaphoreSlim _semaphoreCurrent = new(1);

    // Task Access : Safe

    public async Task StopIndexing(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        Stopwatch timer = Stopwatch.StartNew();

        try
        {
            _queueProcessingEnabled = false;
            await _semaphoreCurrent.WaitAsync(token).ConfigureAwait(false);

            if (!_cts.IsCancellationRequested)
                _cts.Cancel();

            if (_processingTask != null && !_processingTask.IsCompleted)
                await _processingTask.ConfigureAwait(false);
        }
        finally { _semaphoreCurrent.Release(); }

        timer.Stop();
        _logger.LogInformation("StopIndexing operation took {elapsed}.", timer.Elapsed);
    }

    public async Task StartIndexing(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        Stopwatch timer = Stopwatch.StartNew();

        try
        {
            _queueProcessingEnabled = false;
            await _semaphoreCurrent.WaitAsync(token).ConfigureAwait(false);

            if (!_cts.IsCancellationRequested)
                _cts.Cancel();

            if (_processingTask != null && !_processingTask.IsCompleted)
                await _processingTask.ConfigureAwait(false);

            if (token.IsCancellationRequested)
                return;

            _cts = new();
            _processingTask = Reload(_cts.Token);
        }
        finally
        {
            _semaphoreCurrent.Release();
            _queueProcessingEnabled = true;
        }

        timer.Stop();
        _logger.LogInformation("StartIndexing operation took {elapsed}.", timer.Elapsed);
    }

    private void IndexPending()
    {
        if (_queueProcessingEnabled)
            _ = Task.Run(async () => await ProcessPending().ConfigureAwait(false));
    }

    private async Task ProcessPending(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;

        try
        {
            await _semaphoreCurrent.WaitAsync(token).ConfigureAwait(false);

            if (_processingTask != null && !_processingTask.IsCompleted)
                return;

            _cts = new();
            _processingTask = Task.Run(async () => await ProcessByBatches(_cts.Token).ConfigureAwait(false), token);
        }
        finally { _semaphoreCurrent.Release(); }
    }

    // Batch Processing

    private async Task ProcessByBatches(CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();
        IEnumerable<string> pending = Array.Empty<string>();

        while (BuildBatch(pending) is string[] { Length: > 0 } batch)
        {
            ctoken.ThrowIfCancellationRequested();
            pending = await DeepSync(batch, ctoken).ConfigureAwait(false);
        }

        _logger.LogInformation("All pending files have been processed.");
    }

    private string[] BuildBatch(IEnumerable<string> pending)
    {
        List<string> batch = new(pending);
        batch.AddRange(_assetTracking.Dequeue());

        HashSet<string> distinct = batch.ToHashSet(PlatformNamingHelper.FilenameComparer);

        return distinct.ToArray();
    }

    private async Task<IEnumerable<string>> DeepSync(string[] batch, CancellationToken token)
    {
        Stopwatch timer = Stopwatch.StartNew();
        int pathCount = batch.Length;
        int successCount = 0;
        ConcurrentBag<string> pending = new();

        await Parallel.ForEachAsync(batch, token, async (path, token) =>
        {
            if (!await ProcessFile(path, token).ConfigureAwait(false))
            {
                _logger.LogError("Failed to sync file {path}", path);
                pending.Add(path);
            }
            else Interlocked.Increment(ref successCount);
        }).ConfigureAwait(false);
        timer.Stop();
        _logger.LogInformation("Batch processed in {elapsed} ms. No of files: {pathCount}.", timer.Elapsed, pathCount);
        if (successCount < pathCount)
            _logger.LogWarning("Failed to sync {failCount} files.", pathCount - successCount);

        return pending;
    }
}