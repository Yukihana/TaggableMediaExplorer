using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;
using TTX.Data.Models;
using TTX.Library.Comparers;
using TTX.Library.DataHelpers;
using TTX.Library.InstancingHelpers;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    public async partial Task<AssetRecord> Create(IAssetFullSyncInfo syncInfo, AssetMediaInfo mediaInfo, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        // Create
        AssetRecord newRecord = syncInfo.GenerateRecord(mediaInfo);
        byte[] itemId; // TODO add this id first to hashset. if any 'no' hits, only then generate new.

        using (AssetsContext assetsContext = await _dbContextFactory.CreateDbContextAsync(ctoken).ConfigureAwait(false))
        {
            DbSet<AssetRecord> assets = assetsContext.Assets;

            // Get a new id
            HashSet<byte[]> distinct = new(new ByteArrayComparer());
            foreach (AssetRecord rec in await assets.ToListAsync(ctoken).ConfigureAwait(false))
            {
                if (!distinct.Add(rec.ItemId.ToArray()))
                    _logger.LogWarning("Encountered an item ID conflict: {itemId}", rec.ItemId);
            }
            itemId = distinct.GenerateUniqueGuid();

            // Assimilate
            _logger.LogInformation("Creating a record for: {path}", syncInfo.LocalPath);
            await assets.AddAsync(newRecord, ctoken).ConfigureAwait(false);
            await assetsContext.SaveChangesAsync(ctoken).ConfigureAwait(false);
        }

        // Deliver
        return newRecord.DeserializedCopy();
    }
}