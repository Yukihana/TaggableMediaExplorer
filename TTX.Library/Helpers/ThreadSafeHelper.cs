using System;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Library.Helpers;

public static class ThreadSafeHelper
{
    public static async Task SafeApply<T>(this T target, Action<T> action, SemaphoreSlim semaphore, CancellationToken token = default)
    {
        try
        {
            await semaphore.WaitAsync(token).ConfigureAwait(false);
            action.Invoke(target);
        }
        finally { semaphore.Release(); }
    }

    public static async Task<X> SafeGet<T, X>(this T source, Func<T, X> func, SemaphoreSlim semaphore, CancellationToken token = default)
    {
        try
        {
            await semaphore.WaitAsync(token).ConfigureAwait(false);
            return func.Invoke(source);
        }
        finally { semaphore.Release(); }
    }

    // RWLS Read

    public static X SafeRead<T, X>(this T source, Func<T, X> func, ReaderWriterLockSlim rwls)
    {
        try
        {
            rwls.EnterReadLock();
            return func.Invoke(source);
        }
        finally { rwls.ExitReadLock(); }
    }

    // RWLS Write

    public static void SafeWrite<T>(this T source, Action<T> action, ReaderWriterLockSlim rwls)
    {
        try
        {
            rwls.EnterWriteLock();
            action.Invoke(source);
        }
        finally { rwls.ExitWriteLock(); }
    }
}