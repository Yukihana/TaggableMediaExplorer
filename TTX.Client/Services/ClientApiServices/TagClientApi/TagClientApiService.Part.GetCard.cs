using TTX.Client.ViewContexts;
using TTX.Library.Helpers.StringHelpers;

namespace TTX.Client.Services.ClientApiServices.TagClientApi;

internal partial class TagClientApiService
{
    public TagCardContext GetCard(string id)
    {
        return _tagCards.GetOrAdd(
            key: id,
            valueFactory: CreateNew);
    }

    private TagCardContext CreateNew(string id)
    {
        return new()
        {
            TagId = id,
            Title = id.ToTitleFormat(),
            Color = _defaultTagColor,
            Description = "Waiting to sync tag information from the server...",
        };
    }
}