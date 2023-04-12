using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Extensions;
using TTX.Client.Services.ClientSession;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Client.Services.ApiConnection;

internal class ApiConnectionService : IApiConnectionService
{
    private readonly IClientSessionService _clientSession;

    public ApiConnectionService(IClientSessionService sessionClient)
    {
        _clientSession = sessionClient;
    }

    // Base methods

    private async Task<T> Get<T>(string path, string query, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        string? responseString = await _clientSession.Get(path, query, token).ConfigureAwait(false);

        if (string.IsNullOrEmpty(responseString))
            throw new InvalidDataException("Cannot deserialize empty response data.");
        T response = responseString.DeserializeJsonResponse<T>() ??
            throw new NullReferenceException("Deserialization failed.");

        return response;
    }

    // Api : Search requests

    public async Task<SearchResponse> QuerySearch(SearchQuery searchRequest, CancellationToken token = default)
        => await Get<SearchResponse>("api/Search", searchRequest.ToQuery(), token).ConfigureAwait(false);

    // Api : Asset Infos

    public async Task<AssetCardResponse> GetAssetCardData(string idString, CancellationToken ctoken = default)
        => await Get<AssetCardResponse>("api/AssetData/Card", $"id={idString}", ctoken).ConfigureAwait(false);

    // Api : Previews

    public async Task<byte[]> DownloadDefaultPreview(string idString, CancellationToken ctoken = default)
        => await _clientSession.DownloadUsingGet("api/AssetSnapshot", $"id={idString}", ctoken).ConfigureAwait(false);
}