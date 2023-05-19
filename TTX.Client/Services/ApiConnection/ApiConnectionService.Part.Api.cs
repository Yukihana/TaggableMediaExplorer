using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Client.Services.ApiConnection;

internal partial class ApiConnectionService
{
    // Search requests

    public async Task<SearchResponse> QuerySearch(SearchRequest searchRequest, CancellationToken token = default)
        => await _clientSession.GetState<SearchResponse>("api/Search", searchRequest.ToQuery(), token).ConfigureAwait(false);

    // Asset Infos

    public async Task<AssetCardResponse> GetAssetCardData(string idString, CancellationToken ctoken = default)
        => await _clientSession.GetState<AssetCardResponse>("api/AssetData/Card", $"id={idString}", ctoken).ConfigureAwait(false);

    // Previews

    public async Task<byte[]> DownloadDefaultPreview(string idString, CancellationToken ctoken = default)
        => await _clientSession.GetOctet("api/AssetSnapshot", $"id={idString}", ctoken).ConfigureAwait(false);

    // Asset Content

    public async Task<byte[]> DownloadAsset(string idString, CancellationToken ctoken = default)
        => await _clientSession.GetOctet("api/AssetContent", $"id={idString}", ctoken).ConfigureAwait(false);

    // Tagging

    public async Task<RelatedTagsResponse> GetRelatedTags(string searchText, CancellationToken ctoken = default)
        => await _clientSession.GetState<RelatedTagsResponse>("api/RelatedTags", $"searchtext={searchText}", ctoken).ConfigureAwait(false);
}