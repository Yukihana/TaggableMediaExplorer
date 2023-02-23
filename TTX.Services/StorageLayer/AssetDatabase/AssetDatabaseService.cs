using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService : IAssetDatabaseService
{
    // Injected : Primary Services

    private readonly IDbContextFactory<AssetsContext> _dbContextFactory;

    // Injected : Others

    private readonly ILogger<AssetDatabaseService> _logger;
    private readonly AssetDatabaseOptions _options;

    // Internal

    private readonly HashSet<AssetRecord> _cache = new();
    private readonly ReaderWriterLockSlim _lockReadWrite = new();
    private readonly SemaphoreSlim _lockWriteAsync = new(1);

    // Initialization

    public AssetDatabaseService(
        IDbContextFactory<AssetsContext> dbContextFactory,
        ILogger<AssetDatabaseService> logger,
        IOptionsSet options)
    {
        _dbContextFactory = dbContextFactory;

        _logger = logger;
        _options = options.InitializeServiceOptions<AssetDatabaseOptions>();
    }

    // API : Control

    public partial int CacheCount();

    //

    // API : Indexing

    // API : Query

    // Reload

    public partial Task<bool> Reload(CancellationToken ctoken = default);

    private partial Task FixItemIdConflictsInDb(CancellationToken ctoken = default);

    private partial Task LoadRecords(CancellationToken ctoken = default);

    private partial Task<(int, int)> RefreshMemoryCache(IEnumerable<AssetRecord> freshRecords, CancellationToken ctoken = default);

    // Create

    public partial Task<byte[]?> Create(FullAssetSyncInfo info, CancellationToken ctoken = default);

    private partial Task CreateInStorage(AssetRecord rec, CancellationToken ctoken = default);

    private partial Task CreateInMemory(AssetRecord rec, CancellationToken ctoken = default);

    // Update

    public partial Task<bool> Update(byte[] itemId, Action<AssetRecord> updateAction, CancellationToken ctoken = default);

    private partial Task UpdateInStorage(byte[] itemId, Action<AssetRecord> action, CancellationToken ctoken = default);

    private partial void UpdateInMemory(byte[] itemId, Action<AssetRecord> action, CancellationToken ctoken = default);

    // Delete

    public partial Task<bool> Delete(byte[] itemId, CancellationToken ctoken = default);

    private partial Task DeleteInStorage(byte[] itemId, CancellationToken ctoken = default);

    private partial Task DeleteInMemory(byte[] itemId, CancellationToken ctoken = default);

    // Misc

    private partial HashSet<byte[]> GetExistingItemIdsFromCache();

    private partial AssetRecord SelectOneOrThrow(byte[] itemId);

    private static partial Task<AssetRecord> SelectOneOrThrow(AssetsContext context, byte[] itemId, CancellationToken ctoken = default);

    private partial T ReadCacheSafely<T>(Func<T> readFunction);

    private partial Task ModifyCacheSafely(Action action, CancellationToken ctoken = default);
}