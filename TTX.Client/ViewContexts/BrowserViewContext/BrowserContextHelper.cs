using System.Collections.Generic;
using System.Linq;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Client.ViewContexts.BrowserViewContext;

internal static class BrowserContextHelper
{
    // Figure out how to pass string comparer
    public static IEnumerable<string> GetAllTags(this IEnumerable<AssetCardState> states, IEqualityComparer<string> comparer)
    {
        List<string> tags = new();
        foreach (AssetCardState state in states)
            tags.AddRange(state.Tags);
        return tags.ToHashSet(comparer).ToArray();
    }
}