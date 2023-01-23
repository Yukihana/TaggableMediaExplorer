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

        // Ensure unique guids before operations start
        List<byte[]> unique = new();
        List<byte[]> existing = await AssetsTable
            .Select(x => x.GUID)
            .ToListAsync(token)
            .ConfigureAwait(false);
        bool updated = false;

        foreach (AssetRecord asset in AssetsTable)
        {
            if (unique.Any(x => x.SequenceEqual(asset.GUID)))
            {
                byte[] newGuid = EnumerableHelpers.GenerateSafeGuid(existing);

                asset.GUID = newGuid;
                existing.Add(newGuid);
                unique.Add(newGuid);

                updated = true;
            }
            else unique.Add(asset.GUID);
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