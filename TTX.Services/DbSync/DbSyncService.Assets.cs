﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public async Task<bool> AddRecord(AssetRecord rec, CancellationToken token = default)
    {
        try
        {
            using var dbContext = _contextFactory.CreateDbContext();
            DbSet<AssetRecord> AssetsTable = dbContext.Assets;

            // Check for guid uniqueness
            List<byte[]> guids = await AssetsTable.Select(x => x.GUID).ToListAsync(token).ConfigureAwait(false);
            if (guids.Any(x => x.SequenceEqual(rec.GUID)))
                rec.GUID = EnumerableHelpers.GenerateSafeGuid(guids);

            // Add and save changes
            await AssetsTable.AddAsync(rec, token).ConfigureAwait(false);
            await dbContext.SaveChangesAsync(token).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add {type} to the database for path: {path}", nameof(AssetRecord), rec.LastLocation);
            return false;
        }
    }

    public async Task<bool> UpdateRecord(byte[] guid, Action<AssetRecord> action, CancellationToken token = default)
    {
        try
        {
            using var dbContext = _contextFactory.CreateDbContext();
            DbSet<AssetRecord> AssetsTable = dbContext.Assets;
            List<AssetRecord> matched = await AssetsTable
                .Where(x => x.GUID.SequenceEqual(guid))
                .ToListAsync(token)
                .ConfigureAwait(false);

            if (matched.Count == 0)
                throw new InvalidOperationException($"No {nameof(AssetRecord)} exists in database with ID: {new Guid(guid)}");
            else if (matched.Count > 1)
                throw new InvalidOperationException($"Ambiguity between multiple {nameof(AssetRecord)} instances with the same ID: {new Guid(guid)}");

            action.Invoke(matched[0]);
            await dbContext.SaveChangesAsync(token).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update {type} in the database for ID: {guid}", nameof(AssetRecord), new Guid(guid));
            return false;
        }
    }
}