using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.ViewData;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Client.ViewLogic;

public partial class BrowserLogic : ObservableObject
{
    [ObservableProperty]
    private BrowserData _data = new();

    // Async detours

    internal async Task DoSearch(string query, CancellationToken token = default)
        => await SessionContext.DataLoader.DoSearch(query, OnSearch, token).ConfigureAwait(false);

    // Sync updates

    private void OnSearch(SearchResponse response)
    {
        response.Results
            .Select(x => new AssetLogic() { GUID = x })
            .AddTo(Data.Items);
    }
}