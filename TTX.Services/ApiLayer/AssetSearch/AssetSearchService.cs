using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;
using TTX.Services.ProcessingLayer.AssetMetadata;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.ApiLayer.AssetSearch;

public partial class AssetSearchService : IAssetSearchService
{
    private readonly IAssetDatabaseService _assetDatabase;
    private readonly IAssetPresenceService _assetPresence;
    private readonly IAssetMetadataService _assetMetadata;

    public AssetSearchService(
        IAssetDatabaseService assetDatabase,
        IAssetPresenceService assetPresence,
        IAssetMetadataService assetMetadata)
    {
        _assetDatabase = assetDatabase;
        _assetPresence = assetPresence;
        _assetMetadata = assetMetadata;
    }

    public async Task<SearchResponse> Search(SearchRequest query, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        // Prepare materials
        string[] keywords = query.Keywords.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        AssetRecord[] assets = await _assetDatabase.Snapshot(ctoken).ConfigureAwait(false);

        // Isolate targets
        AssetRecord[] queryResults = assets
            // Check keywords first, then check prescence which might require a thread sync wait
            .Where(asset => _assetMetadata.Contains(asset, keywords) && _assetPresence.Get(asset.LocalPath) is not null)
            .ToArray();
        int total = queryResults.Length;

        // Prepare results
        AssetCardState[] finalResults = queryResults
            .Skip(query.Page * query.Count)
            .Take(query.Count)
            .Select(_assetMetadata.CreateAssetCardState)
            .ToArray();
        int shown = finalResults.Length;

        return new(
            Results: finalResults,
            TotalResults: total,
            StartIndex: shown > 0 ? query.Page * query.Count : -1,
            EndIndex: shown > 0 ? query.Page * query.Count + finalResults.Length - 1 : -1
            );
    }
}