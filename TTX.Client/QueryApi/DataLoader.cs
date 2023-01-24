using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.QueryApi;

internal class DataLoader
{
    private readonly SynchronizationContext _syncContext;

    public DataLoader(SynchronizationContext syncContext)
    {
        _syncContext = syncContext;
    }

    internal async Task DoSearch(string query, Action<SearchResponse> dispatchAction, CancellationToken token = default)
    {
        SearchResponse? response = null;
        if (response != null)
            dispatchAction.Invoke(response);
    }

    internal Task LoadAssetCard(string value, Action<AssetCard> loadDataFrom, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}