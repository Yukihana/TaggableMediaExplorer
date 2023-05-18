using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData;
using TTX.Data.ServerData.Entities;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    public async partial Task ScanRepairAnalyse(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();
        _logger.LogInformation("Repair started...");
        AssetRecord[] untracked;

        using (AssetsContext assetsContext = _dbContextFactory.CreateDbContext())
        {
            DbSet<AssetRecord> tracked = assetsContext.Assets;
            if (TrackedAnalysis(tracked, ctoken))
                await assetsContext.SaveChangesAsync(ctoken).ConfigureAwait(false);

            untracked = await assetsContext.Assets.ToArrayAsync(ctoken).ConfigureAwait(false);
        }

        UntrackedAnalysis(untracked, ctoken);
    }

    private partial bool TrackedAnalysis(DbSet<AssetRecord> assets, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();
        int modified = 0;

        if (FixItemIdConflicts(assets, ctoken))
            modified++;

        return modified > 0;
    }

    private partial void UntrackedAnalysis(IEnumerable<AssetRecord> assets, CancellationToken ctoken)
    {
        ScanForIntegrityConflicts(assets, ctoken);
    }
}