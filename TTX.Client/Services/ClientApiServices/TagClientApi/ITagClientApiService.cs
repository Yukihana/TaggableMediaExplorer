using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Client.Services.ClientApiServices.TagClientApi;

internal interface ITagClientApiService
{
    Task<Dictionary<string, string[]>> BulkApplyTags(string[] itemIds, string tagId, bool untag, CancellationToken ctoken = default);

    Task<TagCardState[]> GetTagStates(string[] tagIds, CancellationToken ctoken = default);
}