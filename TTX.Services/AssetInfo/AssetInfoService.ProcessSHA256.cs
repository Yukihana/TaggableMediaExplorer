using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.AssetInfo;

public partial class AssetInfoService
{
    // Implement multiread
    // files below a certain size (100MB) will be fully read, then processed
    // files above, will be 1:1. Utilize Single concurrency semaphore for reads this way.

    public async Task<byte[]?> ComputeSHA256Async(AssetFile file, CancellationToken token = default)
    {
        return file.SizeBytes > _options.SmallComputeMaximumSize
            ? await ComputeSHA256Big(file.FullPath, token).ConfigureAwait(false)
            : await ComputeSHA256Small(file.FullPath, token).ConfigureAwait(false);
    }

    private async Task<byte[]?> ComputeSHA256Small(string path, CancellationToken token = default)
    {
        byte[] data;

        // IO Operation
        try
        {
            await _semaphoreIO.WaitAsync(token).ConfigureAwait(false);
            data = await File.ReadAllBytesAsync(path, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to compute hash for: {path}", path);
            return null;
        }
        finally { _semaphoreIO.Release(); }

        // Processing Operation
        try
        {
            await _semaphoreProc.WaitAsync(token).ConfigureAwait(false);
            byte[]? result = null;
            await Task.Run(() =>
            {
                result = SHA256.HashData(data);
            }, token).ConfigureAwait(false);
            return result;
        }
        finally { _semaphoreProc.Release(); }
    }

    private async Task<byte[]?> ComputeSHA256Big(string path, CancellationToken token = default)
    {
        try
        {
            await _semaphoreIO.WaitAsync(token).ConfigureAwait(false);

            using var fileStream = new FileStream(
                path: path,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.Read,
                bufferSize: _options.ReadBufferSize,
                useAsync: true);

            using var sha256 = SHA256.Create();
            return await sha256.ComputeHashAsync(fileStream, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to compute hash for: {path}", path);
            return null;
        }
        finally { _semaphoreIO.Release(); }
    }

    public async Task<Dictionary<AssetFile, byte[]?>> ComputeSHA256Async(IEnumerable<AssetFile> files, CancellationToken token = default)
    {
        Dictionary<AssetFile, byte[]?> result = new();
        ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = _options.HashProcessingConcurrency,
            CancellationToken = token
        };
        using SemaphoreSlim resultLock = new(1);

        await Parallel.ForEachAsync(files, options,
            async (file, token) =>
            {
                byte[]? hash = await ComputeSHA256Async(file, token).ConfigureAwait(false);

                try
                {
                    await resultLock.WaitAsync(token).ConfigureAwait(false);
                    result[file] = hash;
                }
                finally { resultLock.Release(); }
            }).ConfigureAwait(false);

        return result;
    }
}