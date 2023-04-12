using FFMpegCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.StorageLayer.AssetPreview;

public partial class AssetPreviewService : IAssetPreviewService
{
    private readonly ILogger<AssetPreviewService> _logger;
    private readonly AssetPreviewOptions _options;

    // Options needs AssetValidity

    public AssetPreviewService(ILogger<AssetPreviewService> logger, IOptionsSet options)
    {
#if DEBUG
        string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            ?? throw new NullReferenceException("Cannot acquire path to FFMPEG.");
#else
        string basePath = Environment.CurrentDirectory;
#endif

        string cachePath = Path.Combine(basePath, "ffmpeg", "cache");
        Directory.CreateDirectory(cachePath);

        GlobalFFOptions.Configure(ffmpegoptions =>
        {
            ffmpegoptions.BinaryFolder = Path.Combine(basePath, "ffmpeg", "win64");
            ffmpegoptions.TemporaryFilesFolder = cachePath;
        });

        _logger = logger;
        _options = options.InitializeServiceOptions<AssetPreviewOptions>();
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

        // Temporarily get media length here directly (before MediaAnalysisService is added)
        var mediainfo = FFProbe.Analyse(assetPath);
        TimeSpan mediaLength = mediainfo.Duration;

        // Change this to TimeSpan AssetRecord.SnapshotTime, which should default to medialength * PreviewSnapshotTime when creating the record itself
        TimeSpan snapTime = mediaLength * _options.AssetPreviewSnapshotTime;

        if (!await FFMpeg.SnapshotAsync(assetPath, snapshotPath, captureTime: snapTime).ConfigureAwait(false))
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