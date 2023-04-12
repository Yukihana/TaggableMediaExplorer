using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.MainGui;

public partial class MainLogic
{
    public async Task SearchNew(string keywords, CancellationToken ctoken = default)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            DataModel.Keywords = keywords;
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

            DataModel.PageIndex += 1;
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

            DataModel.PageIndex = DataModel.PageIndex - 1;
            if (DataModel.PageIndex < 0)
                DataModel.PageIndex = 0;
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

            DataModel.PageIndex = pageIndex;
            if (DataModel.PageIndex < 0)
                DataModel.PageIndex = 0;
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

            DataModel.ItemMax = itemCountMax;
            await SearchAsync(ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "The dispatch action for search completed failed from {source}.", ex.Source);
        }
    }
}