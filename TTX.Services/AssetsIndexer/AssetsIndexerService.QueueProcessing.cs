using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

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
            _processingTask = Task.Run(async () => await ProcessAll(_cts.Token).ConfigureAwait(false), token);
        }
        finally { _semaphoreCurrent.Release(); }
    }

    // Tasks

    private async Task ProcessAll(CancellationToken token = default)
    {
        //TODO FIX
        // Change this to parallel, to allow multiple hash calculations together, but allow only one _semCompare for records processing
        // No need for queue (change it back to hashset, if using parallel)

        while (await DequeueAllPending(token).ConfigureAwait(false) is WatcherUpdate[] pending
            && pending.Length > 0
            && !token.IsCancellationRequested)
        {
            await Parallel.ForEachAsync(pending, token,
                async (update, token) => await ProcessUpdate(update, token).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}