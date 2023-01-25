using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Extensions;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Client.ViewLogic;

public partial class MainLogic
{
    private async Task DoSearch(string keywords, int maxcount, int pageindex, CancellationToken token = default)
    {
        SearchQuery query = new()
        {
            Keywords = keywords,
            Count = maxcount,
            Page = pageindex,
        };
        await SessionContext.DataLoader
            .QuerySearch(query, OnSearch, token)
            .ConfigureAwait(false);
    }

    private void OnSearch(SearchResponse response)
    {
        response.Results
            .Select(ModelHelper.CreateAssetLogic)
            .AddTo(DataModel.BrowserLogic.DataModel.Items);
    }

    // Public

    public async Task SearchNew(string keywords, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        await DoSearch(keywords, DataModel.ItemMax, 0, token).ConfigureAwait(false);
    }

    public async Task NextPage(CancellationToken token = default)
    {
    }

    public async Task PreviousPage(CancellationToken token = default)
    {
    }

    public async Task ToPageIndex(int pageIndex, CancellationToken token = default)
    {
    }

    public async Task ResizePage(int itemCountMax, CancellationToken token = default)
    {
    }
}