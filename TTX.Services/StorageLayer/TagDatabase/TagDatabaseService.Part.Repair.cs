using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;

namespace TTX.Services.StorageLayer.TagDatabase;

public partial class TagDatabaseService
{
    public async Task Repair(CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();
        _logger.LogInformation("Repair started...");

        HashSet<string> tagIds = new(StringComparer.OrdinalIgnoreCase);
        List<TagRecord> deleteList = new();

        using (AssetsContext assetsContext = await _dbContextFactory.CreateDbContextAsync(ctoken).ConfigureAwait(false))
        {
            foreach (TagRecord tag in assetsContext.Tags)
            {
                if (!tagIds.Add(tag.TagId))
                    deleteList.Add(tag);
            }

            if (deleteList.Any())
            {
                var deleteIds = deleteList.Select(x => x.TagId).ToArray();
                _logger.LogWarning("Deleting {count} duplicate tags: {list}.", deleteIds.Length, deleteIds);

                foreach (var item in deleteList)
                    assetsContext.Tags.RemoveRange(deleteList);

                await assetsContext.SaveChangesAsync(ctoken).ConfigureAwait(false);
            }
        }

        _logger.LogInformation("Repair completed.");
    }
}