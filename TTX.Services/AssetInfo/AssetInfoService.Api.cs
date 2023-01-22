using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.AssetInfo;

public partial class AssetInfoService
{
    public async Task<AssetFile?> Fetch(string path, bool computeHash = false, CancellationToken token = default)
    {
        try
        {
            await _semaphoreMetadata.WaitAsync(token).ConfigureAwait(false);

            // Metadata
            FileInfo info = new(path);
            AssetFile result = new()
            {
                FullPath = info.FullName,
                CreatedUtc = info.CreationTimeUtc,
                ModifiedUtc = info.LastWriteTimeUtc,
                SizeBytes = info.Length,
            };

            // Crumbs
            long[] crumbsIndices = GetSpreadIndices(result.SizeBytes, _options.CrumbsCount);
            result.Crumbs = await GetCrumbsAsync(path, crumbsIndices, token).ConfigureAwait(false);

            // Hash
            if (computeHash && await ComputeSHA256Async(result, token).ConfigureAwait(false) is byte[] sha256)
                result.SHA256 = sha256;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Error reading metadata", ex);
            return null;
        }
        finally { _semaphoreMetadata.Release(); }
    }

    public async Task<bool> ComputeHash(AssetFile file, CancellationToken token = default)
    {
        byte[]? sha256 = await ComputeSHA256Async(file, token).ConfigureAwait(false);

        if (sha256 == null)
            return false;

        file.SHA256 = sha256;
        return true;
    }
}