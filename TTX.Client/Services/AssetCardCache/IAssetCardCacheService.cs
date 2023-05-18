using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.ViewContexts;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Client.Services.AssetCardCache;

internal interface IAssetCardCacheService
{
    /// <summary>
    /// Gets or creates corresponding AssetCardContext objects.
    /// This does not automatically fetch or update the data.
    /// Caller must keep track and call Update to fetch details from the server.
    /// </summary>
    Dictionary<string, AssetCardContext> Get(string[] idStrings);

    Task Set(AssetCardState[] cards, CancellationToken ctoken = default);
}