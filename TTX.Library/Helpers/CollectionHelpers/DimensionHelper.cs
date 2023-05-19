using System.Collections.Generic;
using System.Linq;

namespace TTX.Library.Helpers.CollectionHelpers;

public static class DimensionHelper
{
    /// <summary>
    /// Recursively extracts all items of type T from the given hierarchy and returns them as a List.
    /// </summary>
    /// <typeparam name="T">The type of item to extract.</typeparam>
    /// <param name="hierarchy">The hierarchy that contains the items.</param>
    /// <returns>A List containing all elements of type T.</returns>
    public static List<T> RecursivelyExtract<T>(this IEnumerable<object> hierarchy)
    {
        List<T> results = hierarchy.OfType<T>().ToList();
        foreach (var item in hierarchy)
        {
            if (item is IEnumerable<object> nestedHierarchy)
                results.AddRange(RecursivelyExtract<T>(nestedHierarchy));
        }
        return results;
    }
}