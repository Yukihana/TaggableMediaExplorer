using System;
using TTX.Data.Entities;

namespace TTX.Services.ApiLayer.AssetSearch;

internal static class AssetSearchHelper
{
    public static bool HasKeywords(this AssetRecord asset, string[] keywords)
    {
        foreach (string keyword in keywords)
        {
            if (asset.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                continue;
            if (asset.TagsString.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                continue;
            return false;
        }
        return true;
    }
}