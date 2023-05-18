using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers.EnumerableHelpers;
using TTX.Library.InstancingHelpers;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.ApiLayer.AssetCardData;

// This class is unused and marked for deletion
public class AssetCardDataService : IAssetCardDataService
{
    private readonly IAssetDatabaseService _assetDatabase;
    private readonly IAssetPresenceService _assetPresence;

    public AssetCardDataService(
        IAssetDatabaseService assetDatabase,
        IAssetPresenceService assetPresence)
    {
        _assetDatabase = assetDatabase;
        _assetPresence = assetPresence;
    }

    public async Task<AssetCardResponse> GetAssetCardData(string id, CancellationToken ctoken = default)
    {
        byte[] itemId = new Guid(id).ToByteArray();
        AssetRecord[] queryResults = Array.Empty<AssetRecord>();

        await _assetDatabase.Read(async (assets) =>
        {
            queryResults = await assets
                .Where(x => x.ItemId.SequenceEqual(itemId))
                .ToArrayAsync(ctoken)
                .ConfigureAwait(false);
        }, ctoken).ConfigureAwait(false);

        return queryResults
            .Where(x => _assetPresence.Get(x.LocalPath)?.SequenceEqual(x.ItemId) == true)
            .SelectOneOrThrow($"Searching for presence match for {id}. ")
            .DeserializedCopyAs<AssetCardResponse>();
    }
}