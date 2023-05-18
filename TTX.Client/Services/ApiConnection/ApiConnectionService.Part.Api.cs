using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.Services.ApiConnection;

internal partial class ApiConnectionService
{
    // Search requests

    public async Task<SearchResponse> QuerySearch(SearchRequest searchRequest, CancellationToken token = default)
        => await Get<SearchResponse>("api/Search", searchRequest.ToQuery(), token).ConfigureAwait(false);

    // Asset Infos

    public async Task<AssetCardResponse> GetAssetCardData(string idString, CancellationToken ctoken = default)
        => await Get<AssetCardResponse>("api/AssetData/Card", $"id={idString}", ctoken).ConfigureAwait(false);

    // Previews

    public async Task<byte[]> DownloadDefaultPreview(string idString, CancellationToken ctoken = default)
        => await _clientSession.GetOctet("api/AssetSnapshot", $"id={idString}", ctoken).ConfigureAwait(false);

    // Asset Content

    public async Task<byte[]> DownloadAsset(string idString, CancellationToken ctoken = default)
        => await _clientSession.GetOctet("api/AssetContent", $"id={idString}", ctoken).ConfigureAwait(false);

    // Asset Tagging

    public async Task<TaggingResponse> BulkApplyTag(TaggingRequest request, CancellationToken ctoken = default)
        => await _clientSession.PostStateToState<TaggingRequest, TaggingResponse>("api/Tagging", request, ctoken).ConfigureAwait(false);

    // Tags

    public async Task<TagCardResponse> GetTagCardData(TagCardRequest request, CancellationToken ctoken = default)
        => await Post<TagCardRequest, TagCardResponse>("api/TagCard", request, null, ctoken).ConfigureAwait(false);

    public async Task<RelatedTagsResponse> GetRelatedTags(string searchText, CancellationToken ctoken = default)
        => await Get<RelatedTagsResponse>("api/RelatedTags", $"searchtext={searchText}", ctoken).ConfigureAwait(false);
}