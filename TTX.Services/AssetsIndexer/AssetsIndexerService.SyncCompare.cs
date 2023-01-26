using System.Linq;
using TTX.Data.Entities;
using TTX.Data.Models;
using TTX.Library.Helpers;

namespace TTX.Services.AssetsIndexer;

// TODO Use AssetRecord.Lock and _semaphoreSync

public partial class AssetsIndexerService
{
    private static bool ProvisionalMatch(AssetRecord rec, AssetFile file, string localPath)
    {
        try
        {
            rec.Lock.EnterReadLock();

            if (rec.SizeBytes != file.SizeBytes)
                return false;
            if (rec.LastLocation != localPath)
                return false;
            if (rec.ModifiedUtc != file.ModifiedUtc)
                return false;
            if (!rec.Crumbs.SequenceEqual(file.Crumbs))
                return false;

            return true;
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    private static bool IntegrityMatch(AssetRecord rec, AssetFile file)
    {
        try
        {
            rec.Lock.EnterReadLock();

            if (rec.SizeBytes != file.SizeBytes)
                return false;
            if (file.SHA256 == null)
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
            return rec.LastLocation.Equals(localPath);
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    private static bool GuidMatch(byte[] guid, AssetRecord rec)
        => rec.SafeRead(x => x.ItemId.SequenceEqual(guid), rec.Lock);
}