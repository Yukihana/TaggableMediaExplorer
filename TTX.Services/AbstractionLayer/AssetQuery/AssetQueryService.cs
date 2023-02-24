using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TTX.Data.Entities;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.AbstractionLayer.AssetQuery;

/// <summary>
/// Keeps track of assets on the disk, and provides query service for server apis.
/// </summary>
public class AssetQueryService : IAssetQueryService
{
    private readonly IAssetDatabaseService _assetDatabase;
    private readonly IAssetPresenceService _assetPresence;

    private readonly ILogger _logger;

    public AssetQueryService(
        IAssetDatabaseService assetDatabase,
        IAssetPresenceService assetPresence,
        ILogger<AssetQueryService> logger)
    {
        _assetDatabase = assetDatabase;
        _assetPresence = assetPresence;
        _logger = logger;
    }

    public TOutput PerformQuery<TInput, TOutput>(TInput input, Func<TInput, IEnumerable<AssetRecord>, TOutput> func)
    {
        return func(input, _assetDatabase.Snapshot().Where(rec => _assetPresence.GetFirst(rec.ItemId) is not null));
    }

    // New version TODO
    /*
    public TOutput PerformQuery<TInput, TOutput>(
        TInput input,
        Func<AssetRecord, TInput, bool> predicate,
        Func<IEnumerable<AssetRecord>, TInput, TOutput> transform)
    {
    }
    */
}