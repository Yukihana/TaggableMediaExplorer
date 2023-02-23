using System.Collections.Generic;
using System.Threading;

namespace TTX.Services.Legacy.Auxiliary;

public partial class AuxiliaryService : IAuxiliaryService
{
    private readonly HashSet<string> _modifiedFiles = new();
    private readonly ReaderWriterLockSlim _lockModifiedFiles = new();

    // Writers

    public bool AddModifiedFiles(string path)
    {
        try
        {
            _lockModifiedFiles.EnterWriteLock();
            return _modifiedFiles.Add(path);
        }
        finally { _lockModifiedFiles.ExitWriteLock(); }
    }

    public bool RemoveModifiedFiles(string path)
    {
        try
        {
            _lockModifiedFiles.EnterWriteLock();
            return _modifiedFiles.Remove(path);
        }
        finally { _lockModifiedFiles.ExitWriteLock(); }
    }

    // Readers (will be needed for query api)

    public int GetModifiedFilesCount()
    {
        try
        {
            _lockModifiedFiles.EnterReadLock();
            return _modifiedFiles.Count;
        }
        finally { _lockModifiedFiles.ExitReadLock(); }
    }
}