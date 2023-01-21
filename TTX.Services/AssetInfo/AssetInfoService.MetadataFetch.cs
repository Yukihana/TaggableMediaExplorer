using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;
using TTX.Library.Helpers;

namespace TTX.Services.AssetInfo;

public partial class AssetInfoService
{
    public async Task<AssetFile?> Fetch(string path, CancellationToken token = default)
    {
        try
        {
            await _semaphore.WaitAsync(token).ConfigureAwait(false);

            FileInfo info = new(path);
            AssetFile result = new()
            {
                FullPath = info.FullName,
                CreatedUtc = info.CreationTimeUtc,
                ModifiedUtc = info.LastWriteTimeUtc,
                SizeBytes = info.Length,
            };

            long[] crumbsIndices = GetSpreadIndices(result.SizeBytes, _options.CrumbsCount);
            result.Crumbs = await GetCrumbsAsync(path, crumbsIndices, token).ConfigureAwait(false);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Error reading metadata", ex);
            return null;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<AssetFile>> Fetch(IEnumerable<string> paths, CancellationToken token = default)
    {
        ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = _options.MetadataConcurrency,
            CancellationToken = token
        };
        ConcurrentBag<AssetFile> results = new();

        await Parallel.ForEachAsync(paths, options,
            async (data, token) => (await Fetch(data, token).ConfigureAwait(false))?.AddTo(results)).ConfigureAwait(false);

        return results.ToList();
    }
}