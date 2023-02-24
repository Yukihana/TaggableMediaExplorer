using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Library.FileSystemHelpers;

namespace TTX.Services.Legacy.AssetsIndexer;

public partial class AssetsIndexerService
{
    // Processing Task

    private CancellationTokenSource _cts = new();
    private Task? _processingTask = null;
    private volatile bool _queueProcessingEnabled = false;
    private readonly SemaphoreSlim _semaphoreCurrent = new(1);

    // Task Access : Safe

    private void IndexPending()
    {
        if (_queueProcessingEnabled)
            _ = Task.Run(async () => await ProcessPending().ConfigureAwait(false));
    }

    // Batch Processing

    private async Task ProcessByBatches(CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();
        IEnumerable<string> pending = Array.Empty<string>();

        while (await BuildBatch(pending, ctoken).ConfigureAwait(false) is string[] { Length: > 0 } batch)
        {
            ctoken.ThrowIfCancellationRequested();
            pending = await _assetSynchronisation.FullSync(batch, ctoken).ConfigureAwait(false);
        }

        _logger.LogInformation("All pending files have been processed.");
    }

    private async Task<string[]> BuildBatch(IEnumerable<string> pending, CancellationToken ctoken = default)
    {
        List<string> batch = new(pending);
        string[] incoming = _assetTracking.Dequeue();

        if (incoming.Any())
            batch.AddRange(incoming);
        else
            await Task.Delay(1000, ctoken).ConfigureAwait(false);

        return batch.ToHashSet(PlatformNamingHelper.FilenameComparer).ToArray();
    }
}