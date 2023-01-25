using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Extensions;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.QueryApi;

internal class DataLoader
{
    private readonly SynchronizationContext _syncContext;
    private readonly ApiClient _client;

    public DataLoader(SynchronizationContext syncContext, ApiClient client)
    {
        _syncContext = syncContext;
        _client = client;
    }

    internal async Task QuerySearch(SearchQuery search, Action<SearchResponse> dispatchAction, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        string responseString = await _client.Get($"api/Search", search.ToQuery(), token).ConfigureAwait(false);
        SearchResponse? response = JsonSerializer.Deserialize<SearchResponse>(responseString);

        _syncContext.Send((state) =>
        {
            if (state is SearchResponse searchResults)
                dispatchAction.Invoke(searchResults);
        }, response);
    }

    internal async Task LoadAssetCard(string guid, Action<AssetCard> dispatchAction, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        string responseString = await _client.Get($"api/AssetData/Card", $"guid={guid}", token).ConfigureAwait(false);
        AssetCard? response = JsonSerializer.Deserialize<AssetCard>(responseString);
        if (response == null)
            return;

        _syncContext.Send((state) =>
        {
            if (state is AssetCard card)
                dispatchAction.Invoke(card);
        }, response);
    }
}