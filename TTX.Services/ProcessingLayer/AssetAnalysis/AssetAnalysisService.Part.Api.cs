using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.ProcessingLayer.AssetAnalysis;

public partial class AssetAnalysisService
{
    public async partial Task<bool> FileExists(string path, CancellationToken token)
    {
        try
        {
            await _semaphoreMetadata.WaitAsync(token).ConfigureAwait(false);
            return File.Exists(path);
        }
        finally { _semaphoreMetadata.Release(); }
    }

    public async partial Task<QuickAssetSyncInfo?> Fetch(string path, string relativeTo, CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();
            return await GetAssetFile<QuickAssetSyncInfo>(path, relativeTo, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error reading metadata at {path}", path);
            return null;
        }
    }

    public async partial Task<FullAssetSyncInfo?> FetchHashed(string path, string relativeTo, CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();
            FullAssetSyncInfo file = await GetAssetFile<FullAssetSyncInfo>(path, relativeTo, token).ConfigureAwait(false);

            // SHA256
            file.SHA256 = file.SizeBytes > _options.ReadBufferSize
                ? await ComputeSHA256Big(file.FullPath, token).ConfigureAwait(false)
                : await ComputeSHA256Small(file.FullPath, token).ConfigureAwait(false);

            return file;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error analysing file at {path}", path);
            return null;
        }
    }

    private async partial Task<T> GetAssetFile<T>(string path, string relativeTo, CancellationToken token) where T : IAssetMetadata, new()
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _semaphoreMetadata.WaitAsync(token).ConfigureAwait(false);

            // Metadata
            FileInfo info = new(path);
            T file = new()
            {
                FullPath = info.FullName,
                LocalPath = Path.GetRelativePath(relativeTo, path),
                CreatedUtc = info.CreationTimeUtc,
                ModifiedUtc = info.LastWriteTimeUtc,
                SizeBytes = info.Length,
            };

            // Crumbs
            long[] crumbsIndices = GetSpreadIndices(file.SizeBytes, _options.CrumbsCount);
            file.Crumbs = await GetCrumbsAsync(path, crumbsIndices, token).ConfigureAwait(false);
            return file;
        }
        finally { _semaphoreMetadata.Release(); }
    }
}