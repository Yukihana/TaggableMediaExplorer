using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;
using TTX.Library.Comparers;
using TTX.Library.DataHelpers;
using TTX.Library.Helpers;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    /// <summary>
    /// Fix ItemId conflicts and return an untracked copy
    /// </summary>
    /// <param name="token">Cancellation Token</param>
    /// <returns>A list of AssetRecords. Empty list on fail.</returns>
    public async partial Task<bool> Reload(CancellationToken ctoken)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            // Fix any possible item id conflicts before runtime.
            await FixItemIdConflictsInDb(ctoken).ConfigureAwait(false);

            // Load records
            await LoadRecords(ctoken).ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database cache refresh failed.");
            return false;
        }
    }

    private async partial Task FixItemIdConflictsInDb(CancellationToken ctoken)
    {
        int updated = 0;
        HashSet<byte[]> existingIds = new(new ByteArrayComparer());

        using var dbContext = _dbContextFactory.CreateDbContext();
        DbSet<AssetRecord> AssetsTable = dbContext.Assets;

        foreach (AssetRecord rec in AssetsTable)
        {
            if (!existingIds.Add(rec.ItemId))
            {
                updated++;
                rec.ItemId = existingIds.GenerateUniqueGuid();
                existingIds.Add(rec.ItemId);
            }
        }

        if (updated > 0)
        {
            await dbContext.SaveChangesAsync(ctoken).ConfigureAwait(false);
            _logger.LogWarning("Resolved {count} redundant ItemIds.", updated);
        }
    }

    private async partial Task LoadRecords(CancellationToken ctoken)
    {
        List<AssetRecord> freshRecords;
        using (AssetsContext dbContext = _dbContextFactory.CreateDbContext())
        {
            freshRecords = await dbContext.Assets.AsNoTracking().ToListAsync(ctoken).ConfigureAwait(false);
        }

        (int, int) report = await RefreshMemoryCache(freshRecords, ctoken).ConfigureAwait(false);
        _logger.LogInformation(
            "Cleared {oldCount} cached records and loading {newCount} records from the storage.",
            report.Item1, report.Item2);
    }

    private async partial Task<(int, int)> RefreshMemoryCache(IEnumerable<AssetRecord> freshRecords, CancellationToken ctoken)
    {
        (int, int) report = new()
        {
            Item2 = freshRecords.Count()
        };

        await ModifyCacheSafely(() =>
        {
            report.Item1 = _cache.Count;
            _cache.Clear();
            freshRecords.AddTo(_cache);
        }, ctoken).ConfigureAwait(false);

        return report;
    }
}