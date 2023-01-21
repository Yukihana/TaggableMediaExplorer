using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TTX.Library.Helpers;

public static class EnumerableHelpers
{
    public static void AddTo<T>(this T item, ICollection<T> collection)
        => collection.Add(item);

    public static void AddTo<T>(this T item, ConcurrentBag<T> collection)
        => collection.Add(item);

    public static void AddTo<T>(this IEnumerable<T> items, ICollection<T> collection)
    {
        foreach (T item in items)
            collection.Add(item);
    }

    public static void AddTo<T>(this IEnumerable<T> items, ConcurrentBag<T> collection)
    {
        foreach (T item in items)
            collection.Add(item);
    }
}