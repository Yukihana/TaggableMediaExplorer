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
using TTX.Services.Integrity;

namespace TTX.Services.Metadata;

public class MetadataService : IMetadataService
{
    private readonly IIntegrityService _integrity;
    private readonly ILogger _logger;

    private readonly MetadataOptions _options;
    private readonly SemaphoreSlim _semaphore;

    public MetadataService(IIntegrityService integrity, ILogger<MetadataService> logger, IOptionsSet options)
    {
        _integrity = integrity;
        _logger = logger;
        _options = options.ExtractValues<MetadataOptions>();
        _semaphore = new SemaphoreSlim(_options.MetadataConcurrency);
    }

    public async Task<AssetFile?> Fetch(string path, CancellationToken token = default)
    {
        try
        {
            await _semaphore.WaitAsync(token).ConfigureAwait(false);

            FileInfo info = new(path);
            return new()
            {
                FullPath = info.FullName,
                CreatedUtc = info.CreationTimeUtc,
                ModifiedUtc = info.LastWriteTimeUtc,
                SizeBytes = info.Length,
                Crumbs = await _integrity.GetCrumbsAsync(path, token).ConfigureAwait(false)
            };
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
            async (data, token) => (await Fetch(data, token))?.AddTo(results));

        return results.ToList();
    }
}