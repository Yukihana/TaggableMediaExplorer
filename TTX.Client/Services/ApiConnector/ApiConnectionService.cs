using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Extensions;
using TTX.Client.Services.SessionClient;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Client.Services.ApiConnector;

internal class ApiConnectionService
{
    private readonly SynchronizationContext _syncContext;
    private readonly SessionClientService _client;

    public ApiConnectionService(SynchronizationContext syncContext, SessionClientService client)
    {
        _syncContext = syncContext;
        _client = client;
    }

    // Base method

    internal async Task PerformGet<T>(string path, string query, Action<T> dispatchAction, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        string? responseString = await _client.Get(path, query, token).ConfigureAwait(false);
        if (string.IsNullOrEmpty(responseString))
            return;

        T? response = responseString.DeserializeJsonResponse<T>();
        if (response == null)
            return;

        _syncContext.Post((state) =>
        {
            if (state is T stateObj)
                dispatchAction.Invoke(stateObj);
        }, response);
    }

    // Serialization scaffold

    internal async Task QuerySearch(SearchQuery search, Action<SearchResponse> dispatchAction, CancellationToken token = default)
        => await PerformGet("api/Search", search.ToQuery(), dispatchAction, token).ConfigureAwait(false);

    internal async Task LoadAssetCard(string idString, Action<AssetCardResponse> dispatchAction, CancellationToken token = default)
        => await PerformGet("api/AssetData/Card", $"id={idString}", dispatchAction, token).ConfigureAwait(false);
}