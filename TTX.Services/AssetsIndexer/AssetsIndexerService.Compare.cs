using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private async Task<bool> QuickMatch(AssetFile file, CancellationToken token = default)
    {
        string localPath = Path.GetRelativePath(_options.AssetsPathFull, file.FullPath);
        foreach (AssetRecord rec in await _records.Snapshot(_semaphoreRecords, token).ConfigureAwait(false))
        {
            try
            {
                await rec.Semaphore.WaitAsync(token).ConfigureAwait(false);

                if (rec.IsValid)
                    continue;
                if (rec.SizeBytes != file.SizeBytes)
                    continue;
                if (rec.LastLocation != localPath)
                    continue;
                if (rec.ModifiedUtc != file.ModifiedUtc)
                    continue;
                if (!rec.Crumbs.SequenceEqual(file.Crumbs))
                    continue;

                rec.IsValid = true;
                return true;
            }
            finally { rec.Semaphore.Release(); }
        }

        return false;
    }

    // Deep Match

    private async Task<bool> DeepMatch(HashedAssetFile file, CancellationToken token = default)
    {
        string localPath = Path.GetRelativePath(_options.AssetsPathFull, file.FullPath);
        foreach (AssetRecord rec in await _records.Snapshot(_semaphoreRecords, token).ConfigureAwait(false))
        {
            try
            {
                await rec.Semaphore.WaitAsync(token).ConfigureAwait(false);

                if (rec.IsValid)
                    continue;
                if (rec.SizeBytes != file.SizeBytes)
                    continue;
                if (!rec.Crumbs.SequenceEqual(file.Crumbs))
                    continue;
                if (!rec.Crumbs.SequenceEqual(file.Crumbs))
                    continue;

                rec.IsValid = true;
                return true;
            }
            finally { rec.Semaphore.Release(); }
        }

        return false;
    }
}