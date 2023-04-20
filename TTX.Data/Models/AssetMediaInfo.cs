using System;

namespace TTX.Data.Models;

public class AssetMediaInfo : IMediaInfo
{
    public string MediaFormat { get; set; } = string.Empty;
    public TimeSpan MediaDuration { get; set; } = TimeSpan.Zero;

    // Primary video track

    public string PrimaryVideoCodec { get; set; } = string.Empty;
    public int PrimaryVideoWidth { get; set; } = 0;
    public int PrimaryVideoHeight { get; set; } = 0;
    public long PrimaryVideoBitRate { get; set; } = 0;

    // Primary audio track

    public string PrimaryAudioCodec { get; set; } = string.Empty;
    public long PrimaryAudioBitRate { get; set; } = 0;

    // Primary subtitle track

    public string PrimarySubtitleCodec { get; set; } = string.Empty;
}