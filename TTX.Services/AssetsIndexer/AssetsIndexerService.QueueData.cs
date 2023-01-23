using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly HashSet<string> _pending = new();

    private readonly ReaderWriterLockSlim _lockPendingList = new();

    // Pending Queue Controls

    private void EnqueuePending(string path)
    {
        try
        {
            _lockPendingList.EnterWriteLock();
            _pending.Add(path);
        }
        finally { _lockPendingList.ExitWriteLock(); }

        if (_queueProcessingEnabled)
            _ = Task.Run(async () => await ProcessPending().ConfigureAwait(false));
    }

    private string[] DequeueAllPending()
    {
        try
        {
            _lockPendingList.EnterWriteLock();
            return _pending.ToArray();
        }
        finally
        {
            _pending.Clear();
            _lockPendingList.ExitWriteLock();
        }
    }

    private void ClearPending()
    {
        try
        {
            _lockPendingList.EnterWriteLock();
            _pending.Clear();
        }
        finally { _lockPendingList.ExitWriteLock(); }
    }

    // Watcher CRUD Sources

    public void OnWatcherEvent(object sender, FileSystemEventArgs e)
        => EnqueuePending(e.FullPath);

    public void OnWatcherEvent(object sender, RenamedEventArgs e)
    {
        EnqueuePending(e.OldFullPath);
        EnqueuePending(e.FullPath);
    }
}