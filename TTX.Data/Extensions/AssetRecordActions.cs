using TTX.Data.Entities;

namespace TTX.Data.Extensions;

public static class AssetRecordActions
{
    public static void SetValid(this AssetRecord rec, bool valid)
    {
        try
        {
            rec.Lock.EnterWriteLock();
            rec.IsValid = valid;
        }
        finally { rec.Lock.ExitWriteLock(); }
    }

    public static bool TryValidate(this AssetRecord rec)
    {
        try
        {
            rec.Lock.EnterUpgradeableReadLock();
            if (rec.IsValid)
                return false;
            rec.SetValid(true);
            return true;
        }
        finally { rec.Lock.ExitUpgradeableReadLock(); }
    }
}