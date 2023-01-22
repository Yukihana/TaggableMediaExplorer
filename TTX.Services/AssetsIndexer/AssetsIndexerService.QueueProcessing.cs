using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
        _logger.LogInformation("StopIndexing operation took {elapsed} milliseconds.", timer.ElapsedMilliseconds);
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
        _logger.LogInformation("StartIndexing operation took {elapsed} milliseconds.", timer.ElapsedMilliseconds);
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

    private async Task ProcessByBatches(CancellationToken token = default)
    {
        while (await DequeueAllPending(token).ConfigureAwait(false) is string[] pending
            && pending.Length > 0
            && !token.IsCancellationRequested)
        {
            await ProcessBatch(pending, token).ConfigureAwait(false);
        }
    }

    private async Task ProcessBatch(string[] pending, CancellationToken token)
    {
        Stopwatch timer = Stopwatch.StartNew();
        int pathCount = pending.Length;
        await Parallel.ForEachAsync(pending, token,
            async (update, token) => await ProcessUpdate(update, token).ConfigureAwait(false)).ConfigureAwait(false);
        timer.Stop();
        _logger.LogInformation("Processing Batch: {pathCount} paths processed in {elapsed} ms.", pathCount, timer.Elapsed);
    }
}