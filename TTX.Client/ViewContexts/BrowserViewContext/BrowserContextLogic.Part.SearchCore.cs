using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
    private async Task SearchAsync(CancellationToken ctoken = default)
    {
        // Make query
        SearchRequest request = await PrepareSearch(ctoken).ConfigureAwait(false);
        SearchResponse response = await SendSearch(request, ctoken).ConfigureAwait(false);

        // Fetch placeholder results
        string[] idStrings = response.Results.Select(x => x.ItemIdString).ToArray();
        Dictionary<string, AssetCardContext> contexts = _assetCardCache.Get(idStrings);
        ObservableCollection<AssetCardContext> collection = new(contexts.Values);

        // Set placeholders using new or stale results
        await UpdateItems(collection, ctoken).ConfigureAwait(false);

        // Refresh context data from the server
        await _assetCardCache.Set(response.Results, ctoken).ConfigureAwait(false);

        // Update thumbnails
        await UpdateThumbnails(contexts, ctoken).ConfigureAwait(false);
    }

    // Prepare request

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

    // Send request

    private async Task<SearchResponse> SendSearch(SearchRequest request, CancellationToken ctoken)
    {
        return await _apiConnection
            .QuerySearch(request, ctoken)
            .ConfigureAwait(false);
    }

    // Allocate

    private async Task UpdateItems(ObservableCollection<AssetCardContext> collection, CancellationToken ctoken = default)
    {
        try
        {
            await _semaphoreResultsDispatch.WaitAsync(ctoken).ConfigureAwait(false);
            ctoken.ThrowIfCancellationRequested();

            await _guiSync.DispatchActionAsync(state =>
            {
                ContextData.Items = state;
            }, collection, ctoken).ConfigureAwait(false);
        }
        finally { _semaphoreResultsDispatch.Release(); }
    }

    // Thumbnails

    private async Task UpdateThumbnails(Dictionary<string, AssetCardContext> contexts, CancellationToken ctoken)
        => await Parallel.ForEachAsync(contexts, ctoken, GetDefaultPreview).ConfigureAwait(false);

    private async ValueTask GetDefaultPreview(KeyValuePair<string, AssetCardContext> state, CancellationToken ctoken = default)
    {
        string idString = state.Key;
        AssetCardContext context = state.Value;

        try
        {
            ctoken.ThrowIfCancellationRequested();

            string thumbPath = await _previewLoader
                .GetDefaultPreview(idString, context.UpdatedUtc, ctoken)
                .ConfigureAwait(false);

            SetDefaultPreviewPath(context, thumbPath, ctoken);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "An error occured while trying to acquire and update the default preview for {id}", idString);
        }
    }

    private void SetDefaultPreviewPath(AssetCardContext context, string thumbPath, CancellationToken ctoken = default)
    {
        _guiSync.DispatchPost(state =>
        {
            try
            {
                state.context.ThumbPath = state.thumbPath;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Failed to update the default preview path: {path}", thumbPath);
            }
        }, (context, thumbPath), ctoken);
    }
}