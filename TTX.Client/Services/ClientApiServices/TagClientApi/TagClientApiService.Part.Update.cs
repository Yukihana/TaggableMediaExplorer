using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.ViewContexts;
using TTX.Data.SharedData.QueryObjects;
using TTX.Library.Helpers.EnumerableHelpers;

namespace TTX.Client.Services.ClientApiServices.TagClientApi;

internal partial class TagClientApiService
{
    public async Task Update(IEnumerable<string> tagIds, CancellationToken ctoken = default)
    {
        string[] idCollection = tagIds.ToArray();

        // Prepare request
        TagCardRequest request = new(
            TagIds: idCollection,
            AutoAdd: true);

        // Make web-api request
        TagCardResponse response = await _clientSession
            .PostStateToState<TagCardRequest, TagCardResponse>("api/Tags/Cards", request, ctoken)
            .ConfigureAwait(false);

        // Validate the response
        if (idCollection.Length != response.Results.Length)
            _logger.LogError("There are inconsistencies in the response. Some tags may not have been fetched successfully: {tags}", response.Failures);

        // Pair and update
        (TagCardState State, TagCardContext Context)[] statePairs = response.Results
            .Select(x => (x, GetCard(x.TagId))).ToArray();

        await UpdateTags(statePairs, 10, ctoken).ConfigureAwait(false);
    }

    private async Task UpdateTags(IEnumerable<(TagCardState, TagCardContext)> statePairs, int batchSize, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        foreach (var batch in statePairs.InBatches(batchSize))
        {
            // dispatch updates for each batch
            await _guiSync.DispatchActionAsync(pairs =>
            {
                foreach (var (state, context) in pairs)
                {
                    try { UpdateContext(state, context); }
                    catch (Exception ex) { _logger.LogError(ex, "Failed to update the context from: {state}", state); }
                }
            }, batch, ctoken).ConfigureAwait(false);
        }
    }

    private static void UpdateContext(TagCardState state, TagCardContext context)
    {
        context.TagId = state.TagId;
        context.Title = state.Title;
        context.Description = state.Description;
        context.Color = state.Color;
    }
}