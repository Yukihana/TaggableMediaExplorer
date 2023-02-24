using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Extensions;
using TTX.Library.Comparers;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService
{
    public async partial Task ReloadRecords(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        await _assetDatabase.Reload(ctoken).ConfigureAwait(false);

        // Mark records with matching asset integrity data (as this may cause duplication issues during sync)
        ScanForDuplicates(ctoken);
    }

    private partial void ScanForDuplicates(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        IEnumerable<AssetRecord> recs = _assetDatabase.Snapshot();
        ConcurrentDictionary<byte[], List<AssetRecord>> results = new(new ByteArrayComparer());

        // Preprocess Keyvalue pairs
        recs.AsParallel().ForAll(rec =>
        {
            byte[] integrityBytes = rec.GetIntegrityBytes();
            results.AddOrUpdate(
                // Key
                integrityBytes,
                // Add factory
                iBytes => new List<AssetRecord>() { rec },
                // Update factory
                (iBytes, list) =>
                {
                    list.Add(rec);
                    return list;
                });
        });

        // Generate report
        StringBuilder sb = new();
        int n = 0;
        foreach (KeyValuePair<byte[], List<AssetRecord>> kvp in results)
        {
            if (kvp.Value.Count > 1)
            {
                n++;
                sb.AppendLine("Found duplicate records");
                foreach (AssetRecord rec in kvp.Value)
                    sb.AppendLine(new Guid(rec.ItemId) + " : " + rec.LocalPath);
            }
        }

        // Finish
        if (n > 0)
            _logger.LogWarning("Founds multiple records with identical integrity information. This may cause synchronisation problems.", sb.ToString());
    }
}