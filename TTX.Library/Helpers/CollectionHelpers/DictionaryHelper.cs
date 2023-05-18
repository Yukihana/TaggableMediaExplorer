using System.Collections.Generic;

namespace TTX.Library.Helpers.CollectionHelpers;

public static class DictionaryHelper
{
    public static (TValue1, TValue2)[] PairValues<TKey, TValue1, TValue2>(
        this Dictionary<TKey, TValue1> dictionary1,
        Dictionary<TKey, TValue2> dictionary2)
        where TKey : notnull
    {
        List<(TValue1, TValue2)> results = new();

        foreach (var kvp1 in dictionary1)
        {
            if (dictionary2.TryGetValue(kvp1.Key, out TValue2? value2))
            {
                var pair = (kvp1.Value, value2);
                results.Add(pair);
            }
        }

        return results.ToArray();
    }
}