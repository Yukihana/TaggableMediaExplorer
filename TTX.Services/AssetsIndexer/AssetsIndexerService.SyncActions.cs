using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private static AssetRecord? FindMatchByDataIntegrity(AssetFullSyncInfo file, IEnumerable<AssetRecord> recs)
    {
        ConcurrentBag<AssetRecord> results = new();
        Parallel.ForEach(recs, rec =>
        {
            if (IntegrityMatch(rec, file))
                results.Add(rec);
        });

        return results.IsEmpty ? null : results.First();
    }

    private static List<AssetRecord> FindMatchByPath(string localPath, IEnumerable<AssetRecord> recs)
    {
        ConcurrentBag<AssetRecord> results = new();
        Parallel.ForEach(recs, rec =>
        {
            if (PathMatch(localPath, rec))
                results.Add(rec);
        });

        return results.ToList();
    }
}