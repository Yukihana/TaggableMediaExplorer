using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Extensions;

namespace TTX.Services.AssetsIndexer;

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

    private void ScanDuplicateRecords()
    {
        ConcurrentBag<(string, AssetRecord)> unordered = new();
        Parallel.ForEach(Snapshot(), x => unordered.Add((x.ToIdentityString(), x)));

        HashSet<string> identities = new();
        HashSet<string> duplicateidentities = new();

        foreach ((string, AssetRecord) element in unordered)
        {
            if (!identities.Add(element.Item1))
                duplicateidentities.Add(element.Item1);
        }

        foreach ((string, AssetRecord) element in unordered)
        {
            if (duplicateidentities.Contains(element.Item1))
                _auxiliary.AddDuplicateRecords(element.Item1, element.Item2);
        }
    }
}