using TTX.Client.ViewContexts;

namespace TTX.Client.Services.AssetCardCache;

internal static class AssetCardCacheHelper
{
    public static AssetCardContext CreateAssetCardContext(this string itemId, string defaultPreviewPath)
    {
        AssetCardContext result = new()
        {
            ItemId = itemId,
            ThumbPath = defaultPreviewPath,
        };
        return result;
    }
}