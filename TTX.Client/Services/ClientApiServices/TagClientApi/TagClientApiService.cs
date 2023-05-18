using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.Services.ClientApiServices.TagClientApi;

internal class TagClientApiService : ITagClientApiService
{
    private readonly IApiConnectionService _apiConnection;
    private readonly ILogger<TagClientApiService> _logger;

    public TagClientApiService(
        IApiConnectionService apiConnection,
        ILogger<TagClientApiService> logger)
    {
        _apiConnection = apiConnection;
        _logger = logger;
    }

    public async Task<Dictionary<string, string[]>> BulkApplyTags(string[] itemIds, string tagId, bool untag, CancellationToken ctoken = default)
    {
        // Prepare request
        TaggingRequest request = new(
            ItemIds: itemIds,
            TagId: tagId,
            Untag: untag);

        // Recieve and parse the data
        TaggingResponse response = await _apiConnection.BulkApplyTag(request, ctoken).ConfigureAwait(false);
        if (response.Failures.Any())
            _logger.LogError("Failed to update the tags for these assets: {failures}", response.Failures);

        return response.Updates;
    }

    public async Task<TagCardState[]> GetTagStates(string[] tagIds, CancellationToken ctoken = default)
    {
        // Prepare request
        TagCardRequest request = new(
            TagIds: tagIds,
            AutoAdd: true);

        // Perform api transaction
        TagCardResponse response = await _apiConnection
            .GetTagCardData(request, ctoken)
            .ConfigureAwait(false);

        // Parse the response
        List<TagCardState> results = new();
        foreach (var item in response.Results)
        {
            if (item.Value is TagCardState state)
                results.Add(state);
        }

        // Error check
        if (tagIds.Length != results.Count)
            _logger.LogError("There are inconsistencies in the response. Some tags may not have been fetched successfully.");

        return results.ToArray();
    }
}