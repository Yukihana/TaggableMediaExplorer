using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Extensions;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Client.ViewLogic;

public partial class MainLogic
{
    private async Task DoSearch(CancellationToken token = default)
    {
        SearchQuery query = new()
        {
            Keywords = DataModel.Keywords,
            Count = DataModel.ItemMax,
            Page = DataModel.PageIndex,
        };
        await SessionContext.ApiConnectionService
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
        DataModel.Keywords = keywords;
        await DoSearch(token).ConfigureAwait(false);
    }

    public async Task NextPage(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        DataModel.PageIndex += 1;
        await DoSearch(token).ConfigureAwait(false);
    }

    public async Task PreviousPage(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        DataModel.PageIndex = DataModel.PageIndex - 1;
        if (DataModel.PageIndex < 0)
            DataModel.PageIndex = 0;
        await DoSearch(token).ConfigureAwait(false);
    }

    public async Task ToPageIndex(int pageIndex, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        DataModel.PageIndex = pageIndex;
        if (DataModel.PageIndex < 0)
            DataModel.PageIndex = 0;
        await DoSearch(token).ConfigureAwait(false);
    }

    public async Task ResizePage(int itemCountMax, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;
        DataModel.ItemMax = itemCountMax;
        await DoSearch(token).ConfigureAwait(false);
    }
}