using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData;
using TTX.Data.ServerData.Entities;

namespace TTX.Services.StorageLayer.TagDatabase;

public partial class TagDatabaseService
{
    public async Task Read(Func<DbSet<TagRecord>, CancellationToken, Task> readAction, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        using AssetsContext assetsContext = await _dbContextFactory.CreateDbContextAsync(ctoken).ConfigureAwait(false);

        await readAction(assetsContext.Tags, ctoken).ConfigureAwait(false);
    }

    public async Task Write(Func<DbSet<TagRecord>, CancellationToken, Task<bool>> writeAction, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        using AssetsContext assetsContext = await _dbContextFactory.CreateDbContextAsync(ctoken).ConfigureAwait(false);

        if (await writeAction(assetsContext.Tags, ctoken).ConfigureAwait(false))
            await assetsContext.SaveChangesAsync(ctoken).ConfigureAwait(false);
    }
}