using FFMpegCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.StorageLayer.MediaCodec;

public partial class MediaCodecService : IMediaCodecService
{
    private readonly ILogger<MediaCodecService> _logger;

    public MediaCodecService(
        ILogger<MediaCodecService> logger,
        IRuntimeConfig config)
    {
        _logger = logger;

        // Initialize FFMPEG

        string cachePath = Path.Combine(config.ServerPath, "ffmpeg", "cache");
        Directory.CreateDirectory(cachePath);

        GlobalFFOptions.Configure(ffmpegoptions =>
        {
            ffmpegoptions.BinaryFolder = Path.Combine(config.ServerPath, "ffmpeg", "win64");
            ffmpegoptions.TemporaryFilesFolder = cachePath;
        });
    }

    public async Task<IMediaAnalysis> Analyse(string path, CancellationToken ctoken = default)
    {
        _logger.LogInformation("Analysing media at {path}.", path);
        return await FFProbe.AnalyseAsync(path, cancellationToken: ctoken).ConfigureAwait(false);
    }

    public async Task<bool> SnapshotAsync(string sourcePath, string snapshotPath, TimeSpan captureTime, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();
        _logger.LogInformation("Generating static snapshot for media at {path}.", sourcePath);
        return await FFMpeg.SnapshotAsync(sourcePath, snapshotPath, captureTime: captureTime).ConfigureAwait(false);
    }

    public async Task<AssetMediaInfo> GetMediaAnalysisInfo(string sourcePath, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();
        _logger.LogInformation("Analysing media information for {path}", sourcePath);

        IMediaAnalysis mediaInfo = await FFProbe.AnalyseAsync(sourcePath, cancellationToken: ctoken).ConfigureAwait(false);
        AssetMediaInfo result = new()
        {
            MediaFormat = mediaInfo.Format.FormatLongName,
            MediaDuration = mediaInfo.Duration,
        };

        if (mediaInfo.PrimaryVideoStream is VideoStream videoStream)
        {
            result.PrimaryVideoCodec = videoStream.CodecLongName;
            result.PrimaryVideoWidth = videoStream.Width;
            result.PrimaryVideoHeight = videoStream.Height;
            result.PrimaryVideoBitRate = videoStream.BitRate;
        }

        if (mediaInfo.PrimaryAudioStream is AudioStream audioStream)
        {
            result.PrimaryAudioCodec = audioStream.CodecLongName;
            result.PrimaryAudioBitRate = audioStream.BitRate;
        }

        if (mediaInfo.PrimarySubtitleStream is SubtitleStream subtitleStream)
        {
            result.PrimarySubtitleCodec = subtitleStream.CodecLongName;
        }

        return result;
    }
}