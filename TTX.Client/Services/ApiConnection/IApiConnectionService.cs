using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.Services.ApiConnection;

internal interface IApiConnectionService
{
    // Search

    Task<SearchResponse> QuerySearch(SearchRequest searchRequest, CancellationToken token = default);

    // Previews

    Task<byte[]> DownloadDefaultPreview(string idString, CancellationToken ctoken = default);

    // Content

    Task<byte[]> DownloadAsset(string idString, CancellationToken ctoken);
}