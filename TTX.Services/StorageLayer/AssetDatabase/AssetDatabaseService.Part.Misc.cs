using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;
using TTX.Data.Extensions;
using TTX.Library.Comparers;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    private partial HashSet<byte[]> GetExistingItemIdsFromCache()
    {
        HashSet<byte[]> existing = new(new ByteArrayComparer());

        try
        {
            _lockReadWrite.EnterReadLock();
            foreach (AssetRecord rec in _cache)
                existing.Add(rec.ItemId.ToArray());
        }
        finally { _lockReadWrite.ExitReadLock(); }
        return existing;
    }

    private static async partial Task<AssetRecord> SelectOneOrThrow(AssetsContext context, byte[] itemId, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        List<AssetRecord> matched = await context.Assets
            .Where(x => x.ItemId.SequenceEqual(itemId))
            .ToListAsync(ctoken)
            .ConfigureAwait(false);
        int count = matched.Count;

        if (count == 0)
            throw new InvalidOperationException($"No record found in storage with Item ID: {new Guid(itemId)}");
        else if (count > 1)
            throw new InvalidOperationException($"{count} (too many) records found in storage with the same Item ID: {new Guid(itemId)}");

        return matched[0];
    }

    private partial AssetRecord SelectOneOrThrow(byte[] itemId)
    {
        List<AssetRecord> matched = new();

        try
        {
            _lockReadWrite.EnterReadLock();
            foreach (AssetRecord rec in _cache)
            {
                if (rec.Read(x => x.ItemId.SequenceEqual(itemId)))
                    matched.Add(rec);
            }
        }
        finally { _lockReadWrite.ExitReadLock(); }
        int count = matched.Count;

        if (count == 0)
            throw new InvalidOperationException($"No record found in cache with Item ID: {new Guid(itemId)}");
        else if (count > 1)
            throw new InvalidOperationException($"{count} (too many) records found in cache with the same Item ID: {new Guid(itemId)}");

        return matched[0];
    }

    // Cache Actions
    private partial T ReadCacheSafely<T>(Func<T> readFunction)
    {
        try
        {
            _lockReadWrite.EnterReadLock();
            return readFunction();
        }
        finally { _lockReadWrite.ExitReadLock(); }
    }

    private async partial Task ModifyCacheSafely(Action action, CancellationToken ctoken)
    {
        try
        {
            await _lockWriteAsync.WaitAsync(ctoken).ConfigureAwait(false);
            _lockReadWrite.EnterWriteLock();

            action.Invoke();
        }
        finally
        {
            _lockReadWrite.ExitWriteLock();
            _lockWriteAsync.Release();
        }
    }

    // Temporary
    public List<AssetRecord> Snapshot()
    {
        try
        {
            _lockReadWrite.EnterReadLock();
            return _cache.ToList();
        }
        finally { _lockReadWrite.ExitReadLock(); }
    }
}