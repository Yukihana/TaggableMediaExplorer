using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly List<WatcherUpdate> _pending = new();

    private readonly SemaphoreSlim _semaphorePendingList = new(1);

    private async Task EnqueuePending(WatcherUpdate update, CancellationToken token = default)
    {
        try
        {
            await _semaphorePendingList.WaitAsync(token).ConfigureAwait(false);
            _pending.Add(update);
        }
        finally { _semaphorePendingList.Release(); }

        if (_queueProcessingEnabled)
            await ProcessPending(token).ConfigureAwait(false);
    }

    private async Task<WatcherUpdate[]> DequeueAllPending(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return Array.Empty<WatcherUpdate>();

        try
        {
            await _semaphorePendingList.WaitAsync(token).ConfigureAwait(false);
            return _pending.ToArray();
        }
        finally
        {
            _pending.Clear();
            _semaphorePendingList.Release();
        }
    }

    private async Task ClearPending(CancellationToken token = default)
    {
        try
        {
            await _semaphorePendingList.WaitAsync(token).ConfigureAwait(false);
            _pending.Clear();
        }
        finally { _semaphorePendingList.Release(); }
    }
}