using Microsoft.Extensions.Logging;
using System;

namespace TTX.Client.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    private async void TagItems()
    {
        try
        {
            await BrowserContext.TagAssets(false, _guiSync.CancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Encountered an error trying to tag items.");
        }
    }

    private async void UntagItems()
    {
        try
        {
            await BrowserContext.TagAssets(true, _guiSync.CancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Encountered an error trying to untag items.");
        }
    }
}