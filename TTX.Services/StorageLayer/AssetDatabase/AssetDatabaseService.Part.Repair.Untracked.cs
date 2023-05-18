using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TTX.Data.ServerData.Entities;
using TTX.Data.ServerData.Extensions;
using TTX.Library.Comparers;
using TTX.Library.Helpers;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    private partial void ScanForIntegrityConflicts(IEnumerable<AssetRecord> assets, CancellationToken ctoken)
    {
        _logger.LogInformation("Scanning cache for integrity conflicts...");

        // Calculate integrity bytes
        ctoken.ThrowIfCancellationRequested();
        List<(byte[], AssetRecord)> calculatedCopy
            = assets
            .AsParallel().AsOrdered()
            .Select(rec => (rec.GetIntegrityBytes(), rec))
            .ToList();

        // Group assets by integrity information
        ctoken.ThrowIfCancellationRequested();
        ConcurrentDictionary<byte[], List<AssetRecord>> results = new(new ByteArrayComparer());
        calculatedCopy.ForEach(item => results.AddOrUpdate(
            key: item.Item1,
            addValueFactory: iBytes => new() { item.Item2 },
            updateValueFactory: (iBytes, list) =>
            {
                list.Add(item.Item2);
                return list;
            }
        ));

        // Generate report
        ctoken.ThrowIfCancellationRequested();
        StringBuilder sb = new();
        int n = 0;
        foreach (var kvp in results.Where(x => x.Value.Count > 1))
        {
            n++;
            string integritySignature = kvp.Key.ToHex(); sb.AppendLine($"Showing duplicates for integrity signature: {integritySignature}");
            foreach (var rec in kvp.Value)
                sb.AppendLine($"> {new Guid(rec.ItemId)} : {rec.LocalPath}");
        }

        // Finish
        if (n > 0)
            _logger.LogWarning("Founds multiple records with identical integrity information. This may cause synchronisation problems.", sb.ToString());
    }
}