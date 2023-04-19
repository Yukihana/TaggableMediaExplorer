using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.InstancingHelpers;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
    private async Task SearchAsync(CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        // Make query
        SearchResponse response = await MakeSearchQuery(ctoken).ConfigureAwait(false);

        try
        {
            await _semaphoreResultsDispatch.WaitAsync(ctoken).ConfigureAwait(false);
            ctoken.ThrowIfCancellationRequested();

            // Allocate
            (string ItemId, AssetCardContext Context)[] cards = await PrepareResults(response, ctoken).ConfigureAwait(false);

            // Load items
            await Parallel.ForEachAsync(cards, ctoken, PopulateContext).ConfigureAwait(false);
        }
        finally { _semaphoreResultsDispatch.Release(); }
    }

    // Query

    private async Task<SearchResponse> MakeSearchQuery(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        SearchQuery request = new()
        {
            Keywords = ContextData.Keywords,
            Count = ContextData.ItemMax,
            Page = ContextData.PageIndex,
        };

        return await _apiConnection
            .QuerySearch(request, ctoken)
            .ConfigureAwait(false);
    }

    // Allocate

    private async Task<(string, AssetCardContext)[]> PrepareResults(SearchResponse response, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        // Add empty results
        List<(string, AssetCardContext)>? results = await _guiSync
            .DispatchFuncAsync(SetResults, response, ctoken).ConfigureAwait(false);

        if (results == null)
            throw new NullReferenceException($"Internal error. Search results collection of {nameof(AssetCardContext)} is null.");

        return results.ToArray();
    }

    private List<(string, AssetCardContext)> SetResults(SearchResponse response)
    {
        List<(string, AssetCardContext)> results = new();
        try
        {
            ObservableCollection<AssetCardContext> collection = new();
            foreach (var result in response.Results)
            {
                AssetCardContext context = new()
                {
                    ItemIdString = result,
                    ThumbPath = Path.Combine(_clientConfig.BaseDirectory, "Resources", "imgwait.png"),
                };

                results.Add((result, context));
                collection.Add(context);
            }

            ContextData.Items = collection;
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Failed to set up search results for display.", ex.Source);
        }
        return results;
    }

    // Query and populate cards

    private async ValueTask PopulateContext((string ItemId, AssetCardContext Context) contextInfo, CancellationToken ctoken = default)
    {
        // Card data
        AssetCardResponse cardData = await _apiConnection.GetAssetCardData(contextInfo.ItemId, ctoken).ConfigureAwait(false);
        _guiSync.DispatchPost(LoadDataFrom, (contextInfo.Context, cardData), ctoken);

        string thumbPath = await _previewLoader.GetDefaultPreview(contextInfo.ItemId, cardData.UpdatedUtc, ctoken).ConfigureAwait(false);
        _guiSync.DispatchPost(LoadSnapshotFrom, (contextInfo.Context, thumbPath), ctoken);
    }

    public static void LoadDataFrom((AssetCardContext Context, AssetCardResponse Response) data)
        => data.Response.CopyPropertiesTo(data.Context);

    public static void LoadSnapshotFrom((AssetCardContext Context, string snapshotPath) data)
        => data.Context.ThumbPath = data.snapshotPath;
}