using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;
using TTX.Library.Helpers;

namespace TTX.Services.Metadata;

public class MetadataService : IMetadataService
{
    private readonly MetadataOptions _options;
    private readonly ILogger _logger;

    private readonly SemaphoreSlim _semaphore;

    public MetadataService(ILogger logger, IOptionsSet options)
    {
        _logger = logger;
        _options = options.ExtractValues<MetadataOptions>();
        _semaphore = new SemaphoreSlim(_options.MetadataConcurrency);
    }

    private static AssetFile ReadFrom(string path)
    {
        FileInfo info = new(path);
        return new AssetFile()
        {
            FullPath = info.FullName,
            CreatedUtc = info.CreationTimeUtc,
            ModifiedUtc = info.LastWriteTimeUtc,
            SizeBytes = info.Length,
        };
    }

    public async Task<AssetFile?> Fetch(string path, CancellationToken token = default)
    {
        try
        {
            await _semaphore.WaitAsync(token).ConfigureAwait(false);
            return await Task.Factory.StartNew(
                () => ReadFrom(path),
                token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Warning, "Error reading metadata", ex);
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