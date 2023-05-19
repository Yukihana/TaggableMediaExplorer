using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
    // Master routine

    private async Task SetSearchResults(SearchResponse response, CancellationToken ctoken = default)
    {
        // Set up asset contexts based on results
        List<(AssetCardState State, AssetCardContext Context)> assetPairs
            = PrepareCards(response.Results);

        // Parallel: Task to update the previews
        var staticPreviewTask = UpdatePreviews(assetPairs, ctoken);

        // Parallel: Set up another task to fetch tags
        IEnumerable<string> allTags = response.Results.GetAllTags(StringComparer.OrdinalIgnoreCase);
        Task cacheTagsTask = _tagClientApi.Update(allTags, ctoken);

        // Allocate assets, Update
        await SetInView(assetPairs.Select(x => x.Context), ctoken).ConfigureAwait(false);
        await UpdateContexts(assetPairs, 5, ctoken).ConfigureAwait(false);

        // Allocate tags for respective assets
        List<(AssetCardContext Context, string[] Tags)> assetTagsInfo = assetPairs
            .Select(pair => (pair.Context, pair.State.Tags)).ToList();
        await AllocateTagsInAssets(assetTagsInfo, ctoken).ConfigureAwait(false);

        // Awaited: Wait for tag updates to complete
        await cacheTagsTask.ConfigureAwait(false);

        // Awaited: Wait for preview updates to finish
        await staticPreviewTask.ConfigureAwait(false);
    }

    private List<(AssetCardState State, AssetCardContext Context)> PrepareCards(IEnumerable<AssetCardState> states)
    {
        List<(AssetCardState State, AssetCardContext Context)> results = new();
        foreach (var state in states)
            results.Add((state, _assetClientApi.GetAssetCard(state.ItemId)));
        return results;
    }

    private async Task SetInView(IEnumerable<AssetCardContext> contexts, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        ObservableCollection<AssetCardContext> collection = new(contexts);
        await _guiSync.DispatchActionAsync(state =>
        {
            try { ContextData.Items = state; }
            catch (Exception ex) { Logger?.LogError(ex, "Failed to update the asset cards in the displayed results."); }
        },
        collection, ctoken).ConfigureAwait(false);
    }

    // Allocations

    private async Task AllocateTagsInAssets(IEnumerable<(string AssetId, string[] TagIds)> updates, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        (AssetCardContext Asset, string[])[] passover = updates
            .Select(x => (_assetClientApi.GetAssetCard(x.AssetId), x.TagIds))
            .ToArray();

        await AllocateTagsInAssets(passover, ctoken).ConfigureAwait(false);
    }

    private async Task AllocateTagsInAssets(IEnumerable<(AssetCardContext Asset, string[] TagIds)> updates, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        //Prepare cards
        (AssetCardContext Asset, ObservableCollection<TagCardContext> Tags)[] contextUpdates = updates.Select(x =>
        {
            TagCardContext[] tagCards = x.TagIds.Select(y => _tagClientApi.GetCard(y)).ToArray();
            return (x.Asset, new ObservableCollection<TagCardContext>(tagCards));
        }).ToArray();

        // Set in gui
        await _guiSync.DispatchActionAsync(pairs =>
        {
            try { foreach (var (asset, tags) in pairs) asset.Tags = tags; }
            catch (Exception ex) { Logger?.LogError(ex, "Failed trying to assign tags to asset cards."); }
        },
        contextUpdates, ctoken).ConfigureAwait(false);
    }
}