using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.ViewContexts;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.Services.TagCardCache;

internal interface ITagCardCacheService
{
    Dictionary<string, TagCardContext> Get(string[] tagIds);

    Task Set(TagCardState[] tags, CancellationToken ctoken = default);
}