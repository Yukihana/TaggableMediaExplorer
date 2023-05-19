using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Client.Services.ApiConnection;

internal interface IApiConnectionService
{
    // Search

    Task<SearchResponse> QuerySearch(SearchRequest searchRequest, CancellationToken token = default);

    // Previews

    Task<byte[]> DownloadDefaultPreview(string idString, CancellationToken ctoken = default);

    // Asset Content

    Task<byte[]> DownloadAsset(string idString, CancellationToken ctoken);

    // Tagging

    Task<RelatedTagsResponse> GetRelatedTags(string searchString, CancellationToken ctoken = default);
}