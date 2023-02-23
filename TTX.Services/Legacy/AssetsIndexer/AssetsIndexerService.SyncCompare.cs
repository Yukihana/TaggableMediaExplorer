using System.Linq;
using TTX.Data.Entities;
using TTX.Data.Extensions;
using TTX.Data.Models;
using TTX.Library.Helpers;

namespace TTX.Services.Legacy.AssetsIndexer;

// TODO Use AssetRecord.Lock and _semaphoreSync

public partial class AssetsIndexerService
{
    private static bool ProvisionalMatch(AssetRecord rec, QuickAssetSyncInfo file)
    {
        // TODO change this to evaluate safely equivalent
        try
        {
            rec.Lock.EnterReadLock();
            return rec.QuickSyncEquals(file);
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    private static bool IntegrityMatch(AssetRecord rec, FullAssetSyncInfo file)
    {
        try
        {
            rec.Lock.EnterReadLock();
            return rec.IntegrityEquals(file);
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    private static bool PathMatch(string localPath, AssetRecord rec)
    {
        try
        {
            rec.Lock.EnterReadLock();
            return rec.LocalPath.Equals(localPath);
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    private static bool ItemIdMatch(byte[] itemId, AssetRecord rec)
        => rec.SafeRead(x => x.ItemId.SequenceEqual(itemId), rec.Lock);
}