using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.ViewContexts;

namespace TTX.Client.Services.ClientApiServices.TagClientApi;

internal interface ITagClientApiService
{
    TagCardContext GetCard(string id);

    Task Update(IEnumerable<string> tagIds, CancellationToken ctoken = default);

    Task<Dictionary<string, string[]>> BulkApplyTags(string[] itemIds, string tagId, bool untag, CancellationToken ctoken = default);
}