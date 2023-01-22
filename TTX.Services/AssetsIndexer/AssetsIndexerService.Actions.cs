using TTX.Data.Entities;
using TTX.Data.Extensions;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private void ValidateOrDuplicate(AssetRecord rec, string path)
    {
        try
        {
            rec.Lock.EnterUpgradeableReadLock();
            if (!rec.IsValid)
                rec.SetValid(true);
            else
                _auxiliary.AddDuplicateFile(path);
        }
        finally { rec.Lock.ExitUpgradeableReadLock(); }
    }
}