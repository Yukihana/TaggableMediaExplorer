using TTX.Data.Encoding;
using TTX.Data.Entities;

namespace TTX.Data.Extensions;

public static partial class AssetRecordExtensions
{
    // String
    public static string ToIdentityString(this AssetRecord rec)
    {
        try
        {
            rec.Lock.EnterReadLock();
            return
                rec.SizeBytes.ToString() + "_" +
                rec.Crumbs.ToHex() + "_" +
                rec.SHA256.ToHex();
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    // Validation

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

    public static bool GetValid(this AssetRecord rec)
    {
        try
        {
            rec.Lock.EnterReadLock();
            return rec.IsValid;
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    public static void SetValid(this AssetRecord rec, bool valid)
    {
        try
        {
            rec.Lock.EnterWriteLock();
            rec.IsValid = valid;
        }
        finally { rec.Lock.ExitWriteLock(); }
    }

    public static void InvalidateByLocalPath(this AssetRecord rec, string localPath)
    {
        try
        {
            rec.Lock.EnterUpgradeableReadLock();
            if (rec.IsValid && rec.LastLocation.Equals(localPath))
                rec.SetValid(false);
        }
        finally { rec.Lock.ExitUpgradeableReadLock(); }
    }
}