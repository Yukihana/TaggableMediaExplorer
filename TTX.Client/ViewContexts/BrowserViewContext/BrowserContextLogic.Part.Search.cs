using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
    private async Task SearchAsync(CancellationToken ctoken = default)
    {
        SearchRequest request = await PrepareSearch(ctoken).ConfigureAwait(false);
        SearchResponse response = await SendSearch(request, ctoken).ConfigureAwait(false);
        await SetSearchResults(response, ctoken).ConfigureAwait(false);
    }

    private async Task<SearchRequest> PrepareSearch(CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        return await _guiSync.DispatchFuncAsync(() =>
        {
            return new SearchRequest(
                Keywords: ContextData.Keywords,
                Page: ContextData.PageIndex,
                Count: ContextData.ItemMax);
        }, ctoken).ConfigureAwait(false)
        ?? throw new InvalidOperationException("Search preparation failed to prepare query object.");
    }

    private async Task<SearchResponse> SendSearch(SearchRequest request, CancellationToken ctoken)
    {
        return await _apiConnection
            .QuerySearch(request, ctoken)
            .ConfigureAwait(false);
    }

    // Specialized wrappers

    public async Task SearchNew(string keywords, CancellationToken ctoken = default)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            ContextData.Keywords = keywords;
            await SearchAsync(ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "The dispatch action for search completed failed from {source}.", ex.Source);
        }
    }

    public async Task NextPage(CancellationToken ctoken = default)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            ContextData.PageIndex += 1;
            await SearchAsync(ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "The dispatch action for search completed failed from {source}.", ex.Source);
        }
    }

    public async Task PreviousPage(CancellationToken ctoken = default)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            ContextData.PageIndex = ContextData.PageIndex - 1;
            if (ContextData.PageIndex < 0)
                ContextData.PageIndex = 0;
            await SearchAsync(ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "The dispatch action for search completed failed from {source}.", ex.Source);
        }
    }

    public async Task ToPageIndex(int pageIndex, CancellationToken ctoken = default)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            ContextData.PageIndex = pageIndex;
            if (ContextData.PageIndex < 0)
                ContextData.PageIndex = 0;
            await SearchAsync(ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "The dispatch action for search completed failed from {source}.", ex.Source);
        }
    }

    public async Task ResizePage(int itemCountMax, CancellationToken ctoken = default)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            ContextData.ItemMax = itemCountMax;
            await SearchAsync(ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "The dispatch action for search completed failed from {source}.", ex.Source);
        }
    }
}