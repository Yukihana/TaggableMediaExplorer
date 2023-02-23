using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Library.Helpers;

namespace TTX.Services.Legacy.DbSync;

public partial class DbSyncService
{
    public async Task<bool> AddRecord(AssetRecord rec, CancellationToken token = default)
    {
        try
        {
            using var dbContext = _contextFactory.CreateDbContext();
            DbSet<AssetRecord> AssetsTable = dbContext.Assets;

            // Check for item id uniqueness
            List<byte[]> itemIds = await AssetsTable.Select(x => x.ItemId).ToListAsync(token).ConfigureAwait(false);
            if (itemIds.Any(x => x.SequenceEqual(rec.ItemId)))
                rec.ItemId = EnumerableHelpers.GenerateSafeItemId(itemIds);

            // Add and save changes
            await AssetsTable.AddAsync(rec, token).ConfigureAwait(false);
            await dbContext.SaveChangesAsync(token).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add {type} to the database for path: {path}", nameof(AssetRecord), rec.LocalPath);
            return false;
        }
    }

    public async Task<bool> UpdateRecord(byte[] itemId, Action<AssetRecord> action, CancellationToken token = default)
    {
        try
        {
            using var dbContext = _contextFactory.CreateDbContext();
            DbSet<AssetRecord> AssetsTable = dbContext.Assets;
            List<AssetRecord> matched = await AssetsTable
                .Where(x => x.ItemId.SequenceEqual(itemId))
                .ToListAsync(token)
                .ConfigureAwait(false);

            if (matched.Count == 0)
                throw new InvalidOperationException($"No {nameof(AssetRecord)} exists in database with ID: {new Guid(itemId)}");
            else if (matched.Count > 1)
                throw new InvalidOperationException($"Ambiguity between multiple {nameof(AssetRecord)} instances with the same ID: {new Guid(itemId)}");

            action.Invoke(matched[0]);
            await dbContext.SaveChangesAsync(token).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update {type} in the database for ID: {itemId}", nameof(AssetRecord), new Guid(itemId));
            return false;
        }
    }
}