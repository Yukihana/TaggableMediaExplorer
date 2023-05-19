using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
    private async Task UpdatePreviews(List<(AssetCardState State, AssetCardContext Context)> resultPairs, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        await Parallel.ForEachAsync(resultPairs, ctoken, async (pair, ct) =>
        {
            try
            {
                ctoken.ThrowIfCancellationRequested();

                await SetDefaultPreview(pair.State, pair.Context, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "An error occured while trying to acquire and update the default preview for {id}", pair.State.ItemId);
            }
        }).ConfigureAwait(false);
    }

    private async ValueTask SetDefaultPreview(AssetCardState state, AssetCardContext context, CancellationToken ctoken = default)
    {
        string thumbPath = await _previewLoader
            .GetDefaultPreview(state.ItemId, context.UpdatedUtc, ctoken)
            .ConfigureAwait(false);

        _guiSync.DispatchPost(data =>
        {
            try
            {
                data.context.ThumbPath = data.thumbPath;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Failed to update the default preview path: {path}", data.thumbPath);
            }
        }, (context, thumbPath), ctoken);
    }
}