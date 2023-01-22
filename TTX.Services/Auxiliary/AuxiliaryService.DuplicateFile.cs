using System.Collections.Generic;
using System.Threading;

namespace TTX.Services.Auxiliary;

public partial class AuxiliaryService : IAuxiliaryService
{
    private readonly HashSet<string> _duplicateFiles = new();
    private readonly ReaderWriterLockSlim _lockDuplicateFiles = new();

    // Writers

    public bool AddDuplicateFile(string path)
    {
        try
        {
            _lockDuplicateFiles.EnterWriteLock();
            return _duplicateFiles.Add(path);
        }
        finally { _lockDuplicateFiles.ExitWriteLock(); }
    }

    public bool RemoveDuplicateFile(string path)
    {
        try
        {
            _lockDuplicateFiles.EnterWriteLock();
            return _duplicateFiles.Remove(path);
        }
        finally { _lockDuplicateFiles.ExitWriteLock(); }
    }

    // Readers (will be needed for query api)
}