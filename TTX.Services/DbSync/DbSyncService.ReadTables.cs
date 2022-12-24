using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Messages;

namespace TTX.Services.DbSync;

public partial class DbSyncService
{
    private async Task ReadAssetsInfoTable(CancellationToken token)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        DbSet<AssetInfo> AssetsTable = dbContext.Assets;
        List<AssetInfo> detachedCopy = await AssetsTable.AsNoTracking().ToListAsync(cancellationToken: token).ConfigureAwait(false);

        var message = new TableData<AssetInfo>()
        {
            TargetSID = _options.AssetsIndexerSID,
            Entities = detachedCopy
        };

        await SendMessage(message, token).ConfigureAwait(false);
    }

    private async Task ReadTagsInfoTable(CancellationToken token)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        DbSet<TagInfo> TagsTable = dbContext.Tags;
        List<TagInfo> detachedCopy = await TagsTable.AsNoTracking().ToListAsync(cancellationToken: token).ConfigureAwait(false);

        var message = new TableData<TagInfo>()
        {
            TargetSID = _options.AssetsIndexerSID,
            Entities = detachedCopy
        };

        await SendMessage(message, token).ConfigureAwait(false);
    }
}