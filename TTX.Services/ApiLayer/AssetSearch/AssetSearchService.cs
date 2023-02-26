using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.ApiLayer.AssetSearch;

public partial class AssetSearchService : IAssetSearchService
{
    private readonly IAssetDatabaseService _assetDatabase;
    private readonly IAssetPresenceService _assetPresence;

    public AssetSearchService(
        IAssetDatabaseService assetDatabase,
        IAssetPresenceService assetPresence)
    {
        _assetDatabase = assetDatabase;
        _assetPresence = assetPresence;
    }

    public async Task<SearchResponse> Search(SearchQuery query, CancellationToken ctoken)
    {
        string[] keywords = query.Keywords.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        AssetRecord[] assets = await _assetDatabase.Snapshot(ctoken).ConfigureAwait(false);

        AssetRecord[] queryResults = assets
            .Where(asset => asset.HasKeywords(keywords) && _assetPresence.Get(asset.LocalPath) is not null)
            .ToArray();
        int total = queryResults.Length;

        string[] finalResults = queryResults
            .Skip(query.Page * query.Count)
            .Take(query.Count)
            .Select(x => new Guid(x.ItemId).ToString())
            .ToArray();
        int shown = finalResults.Length;

        return new()
        {
            Results = finalResults,
            TotalResults = total,
            StartIndex = shown > 0 ? query.Page * query.Count : -1,
            EndIndex = shown > 0 ? query.Page * query.Count + finalResults.Length - 1 : -1,
        };
    }
}