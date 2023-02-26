using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using TTX.Data.Entities;
using TTX.Library.Comparers;
using TTX.Library.DataHelpers;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    private partial bool FixItemIdConflicts(IEnumerable<AssetRecord> assets, CancellationToken ctoken)
    {
        _logger.LogInformation("Attempting to scan and fix item ID conflicts before building the cache...");

        int updated = 0;
        HashSet<byte[]> existingIds = new(new ByteArrayComparer());

        foreach (AssetRecord rec in assets)
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
            _logger.LogWarning("Resolved {count} redundant ItemIds.", updated);
            return true;
        }

        return false;
    }
}