using System.Linq;
using TTX.Data.Entities;
using TTX.Data.Models;
using TTX.Library.Helpers;

namespace TTX.Services.AssetsIndexer;

// TODO Use AssetRecord.Lock and _semaphoreSync

public partial class AssetsIndexerService
{
    private static bool ProvisionalMatch(AssetRecord rec, AssetQuickSyncInfo file)
    {
        try
        {
            rec.Lock.EnterReadLock();

            if (rec.SizeBytes != file.SizeBytes)
                return false;
            if (rec.FilePath != file.LocalPath)
                return false;
            if (rec.ModifiedUtc != file.ModifiedUtc)
                return false;
            if (!rec.Crumbs.SequenceEqual(file.Crumbs))
                return false;

            return true;
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    private static bool IntegrityMatch(AssetRecord rec, AssetFullSyncInfo file)
    {
        try
        {
            rec.Lock.EnterReadLock();

            if (rec.SizeBytes != file.SizeBytes)
                return false;
            if (!rec.Crumbs.SequenceEqual(file.Crumbs))
                return false;
            if (!rec.SHA256.SequenceEqual(file.SHA256))
                return false;

            return true;
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    private static bool PathMatch(string localPath, AssetRecord rec)
    {
        try
        {
            rec.Lock.EnterReadLock();
            return rec.FilePath.Equals(localPath);
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    private static bool ItemIdMatch(byte[] itemId, AssetRecord rec)
        => rec.SafeRead(x => x.ItemId.SequenceEqual(itemId), rec.Lock);
}