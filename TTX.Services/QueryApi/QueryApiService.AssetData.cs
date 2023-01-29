using System;
using System.Linq;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Services.QueryApi;

public partial class QueryApiService
{
    public AssetCardResponse? GetAssetCard(string idString)
    {
        byte[] itemId = new Guid(idString).ToByteArray();
        var results = _assetsIndexer.PerformQuery(itemId, (id, list)
            => list.Where(x => x.ItemId.SequenceEqual(id)));

        if (results.Count() != 1)
            return null;

        return results.First().CopyValues<AssetCardResponse>();
    }
}