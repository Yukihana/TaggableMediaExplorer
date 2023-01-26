using System;
using System.Linq;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Services.QueryApi;

public partial class QueryApiService
{
    public AssetCardResponse? GetAssetCard(string guidString)
    {
        byte[] guid = new Guid(guidString).ToByteArray();
        var results = _assetsIndexer.PerformQuery(guid, (guid, list)
            => list.Where(x => x.ItemId.SequenceEqual(guid)));

        if (results.Count() != 1)
            return null;

        return results.First().CopyValues<AssetCardResponse>();
    }
}