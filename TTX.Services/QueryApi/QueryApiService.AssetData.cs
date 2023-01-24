using System;
using System.Linq;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Services.QueryApi;

public partial class QueryApiService
{
    public AssetCard? GetAssetCard(string guidString)
    {
        byte[] guid = new Guid(guidString).ToByteArray();
        var results = _assetsIndexer.PerformQuery(guid, (guid, list)
            => list.Where(x => x.GUID.SequenceEqual(guid)));

        if (results.Count() != 1)
            return null;

        AssetCard card = results.First().CopyValues<AssetCard>();
        card.GuidString = guidString;
        return card;
    }
}