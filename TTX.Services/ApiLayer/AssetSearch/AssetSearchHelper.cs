using System;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.InstancingHelpers;

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

    public static AssetCardState CreateAssetCardState(this AssetRecord rec)
    {
        AssetCardState result = new() { ItemIdString = new Guid(rec.ItemId).ToString() };
        rec.CopyPropertiesTo(result);
        return result;
    }
}