using System;
using TTX.Data.Entities;

namespace TTX.Data.Extensions;

public static partial class AssetRecordExtensions
{
    /// <summary>
    /// Perform an action in a thread-safe manner using the 'Read' access.
    /// </summary>
    public static void Read(this AssetRecord rec, Action<AssetRecord> readAction)
    {
        try
        {
            rec.Lock.EnterReadLock();
            readAction(rec);
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    /// <summary>
    /// Evaluate a function in a thread-safe manner using the 'Read' access.
    /// </summary>
    public static T Read<T>(this AssetRecord rec, Func<AssetRecord, T> readFunction)
    {
        try
        {
            rec.Lock.EnterReadLock();
            return readFunction(rec);
        }
        finally { rec.Lock.ExitReadLock(); }
    }

    /// <summary>
    /// Perform an action in a thread-safe manner using the 'Write' access.
    /// </summary>
    public static void Write(this AssetRecord rec, Action<AssetRecord> writeAction)
    {
        try
        {
            rec.Lock.EnterWriteLock();
            writeAction(rec);
        }
        finally { rec.Lock.ExitWriteLock(); }
    }

    /// <summary>
    /// Evaluate a function in a thread-safe manner using the 'Write' access.
    /// </summary>
    public static T Write<T>(this AssetRecord rec, Func<AssetRecord, T> writeFunction)
    {
        try
        {
            rec.Lock.EnterWriteLock();
            return writeFunction(rec);
        }
        finally { rec.Lock.ExitWriteLock(); }
    }
}