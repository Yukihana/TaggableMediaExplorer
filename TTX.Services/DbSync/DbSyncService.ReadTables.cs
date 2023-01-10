using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.DbSync;

public partial class DbSyncService
{
    public async Task<List<AssetInfo>> LoadAssets(CancellationToken token)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        DbSet<AssetInfo> AssetsTable = dbContext.Assets;
        return await AssetsTable.AsNoTracking().ToListAsync(cancellationToken: token).ConfigureAwait(false);
    }

    public async Task<List<TagInfo>> LoadTags(CancellationToken token)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        DbSet<TagInfo> TagsTable = dbContext.Tags;
        return await TagsTable.AsNoTracking().ToListAsync(cancellationToken: token).ConfigureAwait(false);
    }
}