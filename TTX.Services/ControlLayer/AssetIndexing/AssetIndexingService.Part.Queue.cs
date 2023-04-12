using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Threading;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.ControlLayer.AssetIndexing;

public partial class AssetIndexingService
{
    public void OnFileSystemEvent(string path, DateTime eventTime)
    {
        try
        {
            // If indexing is not enabled, bail
            _cts.Token.ThrowIfCancellationRequested();
            _logger.LogWarning("Recieved a file system event for the path: {path}", path);

            // Grab the session ID to check past ResetEvent
            int sessionId = GetSessionId();

            // Queue the synchronisation task
            QueueSynchronisationTask(path, sessionId);

            // Piggyback some periodic housekeeping
            ProcessQueue();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to queue synchronisation task for path: {path}", path);
        }
    }

    private async Task FullSyncPath(string path, int sessionId, CancellationToken ctoken)
    {
        // Wait for gate, bail if session has changed
        await _gate.WaitAsync(ctoken).ConfigureAwait(false);
        if (sessionId != GetSessionId())
            return;

        try
        {
            // Wait for semaphore, bail if session has changed
            await _semaphoreSync.WaitAsync(ctoken).ConfigureAwait(false);
            if (sessionId != GetSessionId())
                return;

            // Perform synchronisation
            await _assetSynchronisation.FullSync(path, ctoken: ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to process file event for {path}", path);
        }
        finally { _semaphoreSync.Release(); }
    }

    private int GetSessionId()
        => _gateId;

    private async partial Task WaitForSemaphoreClearance(CancellationToken ctoken)
    {
        while (_semaphoreSync.CurrentCount != _concurrency)
        {
            // WhenAny so that this method isn't held indefinitely
            // by tasks that were blocked before entering the semaphore
            // Important: Do not use this method while the gate is open
            await Task.WhenAny(_tasks).ConfigureAwait(false);

            ProcessQueue();
        }
    }

    // Queue Actions

    private partial Task[] GetActiveTasks(CancellationToken ctoken)
    {
        try
        {
            _lockTasks.EnterReadLock();
            return _tasks.Where(x => !x.IsCompleted).ToArray();
        }
        finally { _lockTasks.ExitReadLock(); }
    }

    private partial void QueueSynchronisationTask(string path, int sessionId)
    {
        try
        {
            _lockTasks.EnterWriteLock();
            _tasks.Add(Task.Run(async () => await FullSyncPath(path, sessionId, _cts.Token).ConfigureAwait(false), _cts.Token));
        }
        finally { _lockTasks.ExitWriteLock(); }
    }

    private void ProcessQueue()
    {
        int completed = 0;
        int count = 0;
        try
        {
            _lockTasks.EnterWriteLock();
            completed = _tasks.RemoveAll(x => x.IsCompleted);
            count = _tasks.Count;
        }
        finally { _lockTasks.ExitWriteLock(); }

        if (completed > 0)
            _logger.LogInformation("Cleared {count} expired task references.", completed);

        if (count == 0)
            _logger.LogInformation("All pending files have been processed.");
    }

    /// <summary>
    /// Ensures cancellation of stale tasks
    /// </summary>
    private void ChangeGateId()
    {
        Interlocked.Increment(ref _gateId);
    }

    private void CreateNewCancellationTokenSource()
    {
        _cts.Dispose();
        _cts = new();
    }
}