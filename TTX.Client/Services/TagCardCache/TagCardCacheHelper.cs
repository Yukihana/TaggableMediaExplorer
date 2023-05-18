using TTX.Client.ViewContexts;
using TTX.Library.Helpers.StringHelpers;

namespace TTX.Client.Services.TagCardCache;

internal static class TagCardCacheHelper
{
    public static TagCardContext CreateTagCardContext(this string tagId, string defaultColor)
    {
        TagCardContext result = new()
        {
            TagId = tagId,
            Title = tagId.ToTitleFormat(),
            Color = defaultColor
        };
        return result;
    }
}