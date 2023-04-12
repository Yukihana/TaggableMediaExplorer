using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.ViewContext;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.InstancingHelpers;

namespace TTX.Client.Services.MainGui;

public partial class MainLogic
{
    private async Task SearchAsync(CancellationToken ctoken = default)
    {
        SearchResponse response = await MakeSearchQuery(ctoken).ConfigureAwait(false);
        (string ItemId, AssetCardContext Context)[] cards = await PrepareResults(response, ctoken).ConfigureAwait(false);
        await PopulateResults(cards, ctoken).ConfigureAwait(false);
    }

    // Query

    private async Task<SearchResponse> MakeSearchQuery(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        SearchQuery request = new()
        {
            Keywords = DataModel.Keywords,
            Count = DataModel.ItemMax,
            Page = DataModel.PageIndex,
        };

        return await _apiConnection
            .QuerySearch(request, ctoken)
            .ConfigureAwait(false);
    }

    // Prepare the contexts for the results

    private async Task<(string, AssetCardContext)[]> PrepareResults(SearchResponse response, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        try
        {
            await _semaphoreResultsDispatch.WaitAsync(ctoken).ConfigureAwait(false);

            // Add empty results
            List<(string, AssetCardContext)>? results = await _guiSync
                .DispatchFuncAsync(SetResults, response, ctoken).ConfigureAwait(false);

            if (results == null)
                throw new NullReferenceException($"Internal error. Search results collection of {nameof(AssetCardContext)} is null.");

            return results.ToArray();
        }
        finally { _semaphoreResultsDispatch.Release(); }
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

            DataModel.BrowserLogic.DataModel.Items = collection;
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Failed to set up search results for display.", ex.Source);
        }
        return results;
    }

    // Query and populate cards

    private async Task PopulateResults((string, AssetCardContext)[] cards, CancellationToken ctoken = default)
    {
        await Parallel.ForEachAsync(cards, ctoken, PopulateContext).ConfigureAwait(false);
        //foreach (AssetCardContext card in cards)
    }

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