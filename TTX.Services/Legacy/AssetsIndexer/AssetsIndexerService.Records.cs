using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TTX.Data.Entities;

namespace TTX.Services.Legacy.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly ReaderWriterLockSlim _lockRecords = new();

    private readonly HashSet<AssetRecord> _records = new();

    private List<AssetRecord> Snapshot()
    {
        try
        {
            _lockRecords.EnterReadLock();
            return _records.ToList();
        }
        finally { _lockRecords.ExitReadLock(); }
    }

    private void SafeAddToRecords(AssetRecord rec)
    {
        try
        {
            _lockRecords.EnterWriteLock();
            _records.Add(rec);
        }
        finally { _lockRecords.ExitWriteLock(); }
    }

    // Query Api

    public TOutput PerformQuery<TInput, TOutput>(TInput input, Func<TInput, IEnumerable<AssetRecord>, TOutput> func)
    {
        try
        {
            _lockRecords.EnterReadLock();

            // Make this read safe for rec.ItemId
            return func(input, _records.Where(rec => _assetPresence.GetFirst(rec.ItemId) is not null));
        }
        finally { _lockRecords.ExitReadLock(); }
    }
}