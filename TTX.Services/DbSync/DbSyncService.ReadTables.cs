using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.DbSync;

public partial class DbSyncService
{
    public async Task<List<AssetRecord>> LoadAssets(CancellationToken token = default)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        DbSet<AssetRecord> AssetsTable = dbContext.Assets;
        return await AssetsTable.AsNoTracking().ToListAsync(cancellationToken: token).ConfigureAwait(false);
    }

    public async Task<List<TagRecord>> LoadTags(CancellationToken token = default)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        DbSet<TagRecord> TagsTable = dbContext.Tags;
        return await TagsTable.AsNoTracking().ToListAsync(cancellationToken: token).ConfigureAwait(false);
    }
}