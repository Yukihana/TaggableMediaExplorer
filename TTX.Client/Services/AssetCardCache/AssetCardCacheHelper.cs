using TTX.Client.ViewContexts;

namespace TTX.Client.Services.AssetCardCache;

internal static class AssetCardCacheHelper
{
    public static AssetCardContext CreateContext(this string idString, string defaultThumbPath)
    {
        AssetCardContext result = new()
        {
            ItemIdString = idString,
            ThumbPath = defaultThumbPath,
            //ThumbPath =
        };
        return result;
    }
}