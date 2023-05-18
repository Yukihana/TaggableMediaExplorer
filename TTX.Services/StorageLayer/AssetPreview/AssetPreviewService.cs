using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData.Entities;
using TTX.Services.StorageLayer.MediaCodec;

namespace TTX.Services.StorageLayer.AssetPreview;

public partial class AssetPreviewService : IAssetPreviewService
{
    private readonly IMediaCodecService _mediaCodec;
    private readonly ILogger<AssetPreviewService> _logger;
    private readonly AssetPreviewOptions _options;

    // Options needs AssetValidity

    public AssetPreviewService(
        IMediaCodecService mediaCodec,
        ILogger<AssetPreviewService> logger,
        IWorkspaceProfile profile,
        IRuntimeConfig config)
    {
        _mediaCodec = mediaCodec;
        _logger = logger;
        _options = profile.InitializeServiceOptions<AssetPreviewOptions>(config);
    }

    // Set

    public async Task Validate(AssetRecord asset, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        if (!Directory.Exists(_options.PreviewsPathFull))
            Directory.CreateDirectory(_options.PreviewsPathFull);

        int updates = 0;

        if (await ValidateSnapshot(asset, ctoken).ConfigureAwait(false))
            updates++;

        if (updates != 1)
            _logger.LogError("All previews could not be accounted for. Asset: {id} at {path}", asset.ItemId, asset.LocalPath);
    }

    private async Task<bool> ValidateSnapshot(AssetRecord asset, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        string snapshotPath = Path.Combine(_options.PreviewsPathFull, new Guid(asset.ItemId).ToString());

        if (File.Exists("name") &&
            new FileInfo("name") is FileInfo f &&
            f.CreationTimeUtc > asset.UpdatedUtc &&
            f.Length != 0)
            return true;

        string assetPath = Path.Combine(_options.AssetsPathFull, asset.LocalPath);

        // Change this to TimeSpan AssetRecord.SnapshotTime,
        // which should default to medialength * PreviewSnapshotTime when creating the record itself
        TimeSpan snapTime = asset.MediaDuration * _options.AssetPreviewSnapshotTime;

        if (!await _mediaCodec.SnapshotAsync(assetPath, snapshotPath, snapTime, ctoken).ConfigureAwait(false))
        {
            _logger.LogError("Failed to create preview snapshot for {path}", asset.LocalPath);
            return false;
        }

        return true;
    }

    // Get

    public string? GetSnapshotPath(string idString)
    {
        string path = GetSnapshotPathFromId(idString);
        return File.Exists(path) ? path : null;
    }

    private string GetSnapshotPathFromId(string idString)
        => Path.Combine(_options.PreviewsPathFull, idString + ".png");
}