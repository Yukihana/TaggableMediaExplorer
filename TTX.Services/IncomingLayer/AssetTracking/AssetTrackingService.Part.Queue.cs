namespace TTX.Services.IncomingLayer.AssetTracking;

public partial class AssetTrackingService
{
    private partial void EnqueueValidated(params string[] paths)
    {
        try
        {
            _lockQueue.EnterWriteLock();
            foreach (string path in paths)
            {
                if (ValidatePathByPattern(path))
                    _queue.Add(path);
            }
        }
        finally { _lockQueue.ExitWriteLock(); }

        _onEnqueueAction?.Invoke();
    }

    public partial string[] Dequeue()
    {
        try
        {
            _lockQueue.EnterWriteLock();
            return _queue.ToArray();
        }
        finally
        {
            _queue.Clear();
            _lockQueue.ExitWriteLock();
        }
    }

    public partial void ClearPending()
    {
        try
        {
            _lockQueue.EnterWriteLock();
            _queue.Clear();
        }
        finally { _lockQueue.ExitWriteLock(); }
    }
}