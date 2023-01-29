using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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

    public static byte[] GenerateSafeItemId(IEnumerable<byte[]> existing)
    {
        byte[] result;
        do { result = Guid.NewGuid().ToByteArray(); }
        while (existing.Any(x => x.SequenceEqual(result)));
        return result;
    }
}