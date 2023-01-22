using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly HashSet<string> _pending = new();

    private readonly SemaphoreSlim _semaphorePendingList = new(1);

    // Pending Queue Controls

    private async Task EnqueuePending(string path, CancellationToken token = default)
    {
        try
        {
            await _semaphorePendingList.WaitAsync(token).ConfigureAwait(false);
            _pending.Add(path);
        }
        finally { _semaphorePendingList.Release(); }

        if (_queueProcessingEnabled)
            await ProcessPending(token).ConfigureAwait(false);
    }

    private async Task<string[]> DequeueAllPending(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return Array.Empty<string>();

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

    // Watcher CRUD Sources

    public async void OnWatcherEvent(object sender, FileSystemEventArgs e)
        => await EnqueuePending(e.FullPath).ConfigureAwait(false);

    public async void OnWatcherEvent(object sender, RenamedEventArgs e)
    {
        await EnqueuePending(e.OldFullPath).ConfigureAwait(false);
        await EnqueuePending(e.FullPath).ConfigureAwait(false);
    }
}