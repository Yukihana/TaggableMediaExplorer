using System;
using System.Linq;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;
using TTX.Services.AbstractionLayer.AssetQuery;

namespace TTX.Services.ApiLayer.AssetCardData;

public class AssetCardDataService : IAssetCardDataService
{
    private readonly IAssetQueryService _assetQuery;

    public AssetCardDataService(IAssetQueryService assetQuery)
    {
        _assetQuery = assetQuery;
    }

    public AssetCardResponse? GetAssetCardData(string id)
    {
        byte[] itemId = new Guid(id).ToByteArray();
        var results = _assetQuery.PerformQuery(itemId, (id, list)
            => list.Where(x => x.ItemId.SequenceEqual(id)));

        if (results.Count() != 1)
            return null;

        return results.First().CopyByReflection<AssetCardResponse>();
    }
}