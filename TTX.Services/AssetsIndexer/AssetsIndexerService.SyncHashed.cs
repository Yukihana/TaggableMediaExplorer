using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Extensions;
using TTX.Data.Models;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly SemaphoreSlim _semaphoreSync = new(1);

    private async Task ProcessUpdate(string path, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return;

        // Fetch info
        AssetFile? file = await _assetInfo.Fetch(path, true, token).ConfigureAwait(false);

        // If file is inaccessible, invalidate
        if (file == null)
        {
            InvalidatePath(path);
            return;
        }

        // Legacy
        if (!await ProcessFile(path, token).ConfigureAwait(false))
            _logger.LogError("Failed to sync file {path}", path);
    }

    // Asset validation
    private void InvalidatePath(string path)
    {
        Parallel.ForEach(Snapshot(),
            x => x.InvalidateByLocalPath(path));

        _auxiliary.RemoveDuplicateFile(path);
    }

    // Unknown

    private async Task<bool> ProcessFile(string path, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            return false;

        try
        {
            await _semaphoreCurrent.WaitAsync(token).ConfigureAwait(false);

            // Generate Metadata
            AssetFile file = new();

            // Generate Hash
            AssetFile hfile = new();

            // Fetch invalids
            HashSet<AssetRecord> recs = Snapshot().ToHashSet();

            // Match by: Size, Crumbs, Hash, Filename => Instant match

            //

            // If all else fails create new record
            await CreateRecord(hfile, token).ConfigureAwait(false);
        }
        finally { _semaphoreCurrent.Release(); }

        return false;
    }
}