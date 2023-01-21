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
            await semaphore.WaitAsync(token);
            action.Invoke(target);
        }
        finally { semaphore.Release(); }
    }

    public static async Task<X> SafeExecute<T, X>(this T source, Func<T, X> func, SemaphoreSlim semaphore, CancellationToken token = default)
    {
        try
        {
            await semaphore.WaitAsync(token);
            return func.Invoke(source);
        }
        finally { semaphore.Release(); }
    }
}