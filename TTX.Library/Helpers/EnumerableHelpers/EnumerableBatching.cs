using System.Collections.Generic;

namespace TTX.Library.Helpers.EnumerableHelpers;

public static class EnumerableBatching
{
    public static IEnumerable<List<T>> InBatches<T>(this IEnumerable<T> collection, int batchSize)
    {
        List<T> batch = new(batchSize);

        foreach (T item in collection)
        {
            batch.Add(item);

            if (batch.Count == batchSize)
            {
                yield return batch;
                batch = new List<T>(batchSize);
            }
        }

        if (batch.Count > 0)
            yield return batch;
    }
}