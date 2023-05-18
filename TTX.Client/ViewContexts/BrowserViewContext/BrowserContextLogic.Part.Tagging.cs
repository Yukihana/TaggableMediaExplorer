using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.SharedData.QueryObjects;
using TTX.Library.Helpers.CollectionHelpers;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
    public async Task TagAssets(bool untag, CancellationToken ctoken = default)
    {
        // Get tag from modal
        ctoken.ThrowIfCancellationRequested();
        string? userInput = await _tagSelectorGui.ShowModalAsync(ctoken).ConfigureAwait(false);

        // Apply tagging operation
        if (userInput is not string tagId)
            return;

        // Prepare request
        Dictionary<string, AssetCardContext> selected
            = await GetSelectedAssetCards(ctoken)
            .ConfigureAwait(false);
        string[] idStrings = selected.Keys.ToArray();

        // Parse response
        Dictionary<string, string[]> updates = await _tagClientApi
            .BulkApplyTags(idStrings, tagId, untag, ctoken)
            .ConfigureAwait(false);

        // Kick off updating the TagCard cache
        string[][] allTags = updates.Values.ToArray();
        Task tagsTask = UpdateTagCards(allTags, ctoken);

        // Assign corresponding TagCardContexts to AssetCardContexts
        // Make sure to try-catch this part, else on crash tagsTask finish won't be reached properly either
        try
        {
            (AssetCardContext, string[])[] assetCardUpdates = selected.PairValues(updates);
            await UpdateAssetCardTags(assetCardUpdates, ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Failed to update AssetCardContexts' tag information. Tags: {tags}", updates);
        }

        // Confirm tags update task has completed
        await tagsTask.ConfigureAwait(false);
    }

    private async Task<Dictionary<string, AssetCardContext>> GetSelectedAssetCards(CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        // Quick grab data on gui thread to reduce overhead
        List<(string, AssetCardContext)> rawListing = await _guiSync.DispatchFuncAsync(
            () => ContextData.SelectedItems.Select(x => (x.ItemId, x)).ToList(),
            ctoken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Unable to fetch data from the main thread.");

        // Create dictionary on non-gui thread
        return rawListing.ToDictionary(
            x => x.Item1,
            x => x.Item2,
            StringComparer.OrdinalIgnoreCase);
    }

    private async Task UpdateAssetCardTags(
        (AssetCardContext, string[])[] assetCardUpdates,
        CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        foreach (var item in assetCardUpdates)
        {
            await _guiSync.DispatchActionAsync(x =>
            {
                (AssetCardContext context, string[] tags) = x;
                ObservableCollection<TagCardContext> collection = new(_tagCardCache.Get(tags).Values);
                context.Tags = collection;
            }, item, ctoken).ConfigureAwait(false);
        }
    }

    private async Task UpdateTagCards(
        string[][] allTags,
        CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        // Prepare data
        List<string> tagsDump = new();
        foreach (var tagArray in allTags)
            tagsDump.AddRange(tagArray);
        string[] tags = tagsDump
            .ToHashSet(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        await UpdateTagCards(tags, ctoken)
            .ConfigureAwait(false);
    }

    private async Task UpdateTagCards(
        string[] tags,
        CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        // Make api request
        TagCardState[] states = await _tagClientApi
            .GetTagStates(tags, ctoken)
            .ConfigureAwait(false);

        // Update tag states
        await _tagCardCache.Set(states, ctoken).ConfigureAwait(false);
    }
}