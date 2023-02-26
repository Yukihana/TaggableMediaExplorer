using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    public async partial Task Read(Func<DbSet<AssetRecord>, Task> readAction, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();
        using AssetsContext assetsContext = await _dbContextFactory.CreateDbContextAsync(ctoken).ConfigureAwait(false);
        await readAction(assetsContext.Assets).ConfigureAwait(false);
    }

    public async partial Task Write(Func<DbSet<AssetRecord>, Task<bool>> writeAction, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();
        using AssetsContext assetContext = _dbContextFactory.CreateDbContext();

        if (await writeAction(assetContext.Assets).ConfigureAwait(false))
            await assetContext.SaveChangesAsync(ctoken).ConfigureAwait(false);
    }

    public async partial Task<AssetRecord[]> Snapshot(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();
        using AssetsContext assetsContext = await _dbContextFactory.CreateDbContextAsync(ctoken).ConfigureAwait(false);
        return await assetsContext.Assets.AsNoTracking().ToArrayAsync(ctoken).ConfigureAwait(false);
    }
}