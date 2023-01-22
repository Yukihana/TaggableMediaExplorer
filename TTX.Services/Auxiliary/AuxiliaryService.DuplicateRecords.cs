using System.Collections.Generic;
using System.Threading;
using TTX.Data.Entities;

namespace TTX.Services.Auxiliary;

public partial class AuxiliaryService : IAuxiliaryService
{
    private readonly Dictionary<string, HashSet<AssetRecord>> _duplicateRecords = new();
    private readonly ReaderWriterLockSlim _lockDuplicateRecords = new();

    // Writers

    public bool AddDuplicateRecords(string identity, AssetRecord rec)
    {
        try
        {
            _lockDuplicateRecords.EnterWriteLock();
            if (_duplicateRecords.TryGetValue(identity, out HashSet<AssetRecord>? value) && value != null)
                return value.Add(rec);
            else
                _duplicateRecords.Add(identity, new() { rec });
            return true;
        }
        finally { _lockDuplicateRecords.ExitWriteLock(); }
    }

    public bool RemoveDuplicateRecords(AssetRecord record)
    {
        bool result = false;
        try
        {
            _lockDuplicateRecords.EnterUpgradeableReadLock();
            foreach (KeyValuePair<string, HashSet<AssetRecord>> recs in _duplicateRecords)
            {
                if (recs.Value.Contains(record))
                {
                    recs.Value.Remove(record);

                    if (recs.Value.Count < 2)
                    {
                        try
                        {
                            _lockDuplicateRecords.EnterWriteLock();
                            _duplicateRecords.Remove(recs.Key);
                        }
                        finally { _lockDuplicateRecords.ExitWriteLock(); }
                    }
                    result = true;
                }
            }
        }
        finally { _lockDuplicateRecords.ExitUpgradeableReadLock(); }

        return result;
    }

    // Readers (will be needed for query api)
}