using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;
using TTX.Data.Extensions;
using TTX.Data.Models;
using TTX.Library.DataHelpers;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    public async partial Task<byte[]?> Create(FullAssetSyncInfo info, CancellationToken ctoken)
    {
        string path = info.LocalPath;

        try
        {
            ctoken.ThrowIfCancellationRequested();

            AssetRecord newRecord = info.GenerateRecord();
            newRecord.ItemId = GetExistingItemIdsFromCache().GenerateUniqueGuid();

            await CreateInStorage(newRecord, ctoken).ConfigureAwait(false);
            await CreateInMemory(newRecord, ctoken).ConfigureAwait(false);
            _logger.LogWarning("Created record for asset at path: {path}", path);
            return newRecord.ItemId.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create record for: {path}", path);
            return null;
        }
    }

    private async partial Task CreateInStorage(AssetRecord rec, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        using AssetsContext dbContext = await _dbContextFactory.CreateDbContextAsync(ctoken).ConfigureAwait(false);
        await dbContext.Assets.AddAsync(rec, ctoken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(ctoken).ConfigureAwait(false);
    }

    private async partial Task CreateInMemory(AssetRecord rec, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        await ModifyCacheSafely(() => _cache.Add(rec), ctoken).ConfigureAwait(false);
    }
}