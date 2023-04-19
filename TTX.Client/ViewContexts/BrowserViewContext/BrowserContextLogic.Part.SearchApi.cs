using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
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