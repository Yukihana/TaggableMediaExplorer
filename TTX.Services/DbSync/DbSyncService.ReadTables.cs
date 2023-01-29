using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Library.Helpers;

namespace TTX.Services.DbSync;

public partial class DbSyncService
{
    public async Task<List<AssetRecord>> LoadAssets(CancellationToken token = default)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        DbSet<AssetRecord> AssetsTable = dbContext.Assets;

        // Ensure unique ItemIds before operations start
        List<byte[]> unique = new();
        List<byte[]> existing = await AssetsTable
            .Select(x => x.ItemId)
            .ToListAsync(token)
            .ConfigureAwait(false);
        bool updated = false;

        foreach (AssetRecord asset in AssetsTable)
        {
            if (unique.Any(x => x.SequenceEqual(asset.ItemId)))
            {
                byte[] newItemId = EnumerableHelpers.GenerateSafeItemId(existing);

                asset.ItemId = newItemId;
                existing.Add(newItemId);
                unique.Add(newItemId);

                updated = true;
            }
            else unique.Add(asset.ItemId);
        }

        if (updated)
            await dbContext.SaveChangesAsync(token).ConfigureAwait(false);

        // Return an untracked copy
        return await AssetsTable.AsNoTracking().ToListAsync(token).ConfigureAwait(false);
    }

    public async Task<List<TagRecord>> LoadTags(CancellationToken token = default)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        DbSet<TagRecord> TagsTable = dbContext.Tags;
        return await TagsTable.AsNoTracking().ToListAsync(cancellationToken: token).ConfigureAwait(false);
    }
}