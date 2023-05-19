using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Client.Services.ClientApiServices.TagClientApi;

internal partial class TagClientApiService
{
    public async Task<Dictionary<string, string[]>> BulkApplyTags(string[] itemIds, string tagId, bool untag, CancellationToken ctoken = default)
    {
        // Prepare request
        TaggingRequest request = new(
            ItemIds: itemIds,
            TagId: tagId,
            Untag: untag);

        // Make web-api request
        TaggingResponse response = await _clientSession
            .PostStateToState<TaggingRequest, TaggingResponse>("api/Tagging", request, ctoken)
            .ConfigureAwait(false);

        // Validate and return
        if (response.Failures.Any())
            _logger.LogError("Failed to update the tags for these assets: {failures}", response.Failures);

        return response.Updates;
    }
}