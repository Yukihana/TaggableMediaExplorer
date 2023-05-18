using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Services.ApiLayer.TagData;

public interface ITagDataService
{
    Task<RelatedTagsResponse> GetRelatedTags(string searchText, CancellationToken ctoken = default);

    Task<TagCardResponse> GetCards(TagCardRequest request, CancellationToken ctoken = default);
}