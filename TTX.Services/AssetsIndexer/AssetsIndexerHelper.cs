using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Library.Helpers;

namespace TTX.Services.AssetsIndexer;

public static class AssetsIndexerHelper
{
    // Snapshot

    public static async Task<List<AssetRecord>> Snapshot(this HashSet<AssetRecord> recs, SemaphoreSlim semaphore, CancellationToken token = default)
        => await recs.SafeExecute(x => x.ToList(), semaphore, token);

    // Validation

    public static async Task Validate(this AssetRecord rec, CancellationToken token = default)
        => await rec.SafeApply(x => x.IsValid = true, rec.Semaphore, token);

    public static async Task Invalidate(this AssetRecord rec, CancellationToken token = default)
        => await rec.SafeApply(x => x.IsValid = false, rec.Semaphore, token);
}