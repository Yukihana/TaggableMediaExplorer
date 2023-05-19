using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Library.Helpers.CollectionHelpers;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
    // Master routine

    public async Task TagAssets(bool untag, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        // Prepare and validate input
        string? userInput = await _tagSelectorGui.ShowModalAsync(ctoken).ConfigureAwait(false);
        string[] assetIds = await GetSelectedAssetIds(ctoken).ConfigureAwait(false);

        if (userInput is not string tagId || !assetIds.Any())
            return;

        // Make web-api request
        Dictionary<string, string[]> updates = await _tagClientApi
            .BulkApplyTags(assetIds, tagId, untag, ctoken)
            .ConfigureAwait(false);

        // Get updated states of associated tags
        HashSet<string> allTags = updates.Values
            .RecursivelyExtract<string>()
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        Task cacheTask = _tagClientApi.Update(allTags, ctoken);

        // Allocate and set tags in view
        await AllocateTagsInAssets(
            updates.Select(x => (x.Key, x.Value)),
            ctoken).ConfigureAwait(false);

        // Confirm tags update task has completed
        await cacheTask.ConfigureAwait(false);
    }

    private async Task<string[]> GetSelectedAssetIds(CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        string[]? result = await _guiSync.DispatchFuncAsync(context =>
        {
            try { return context.SelectedItems.Select(x => x.ItemId).ToArray(); }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Unable to fetch selected asset ids from the main thread.");
                return null;
            }
        },
        ContextData, ctoken).ConfigureAwait(false);

        return result ?? throw new Exception("Failed to get selected asset ids.");
    }
}