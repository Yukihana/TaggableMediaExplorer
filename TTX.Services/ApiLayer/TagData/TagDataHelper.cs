using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.InstancingHelpers;

namespace TTX.Services.ApiLayer.TagData;

internal static class TagDataHelper
{
    public static TagCardState ToState(this TagRecord tag)
        => tag.DeserializedCopyAs<TagCardState>();
}