using System;

namespace TTX.Data.ServerData.Models;

public interface IMediaInfo
{
    string MediaFormat { get; set; }
    TimeSpan MediaDuration { get; set; }

    // Primary video track

    string PrimaryVideoCodec { get; set; }
    int PrimaryVideoWidth { get; set; }
    int PrimaryVideoHeight { get; set; }
    long PrimaryVideoBitRate { get; set; }

    // Primary audio track

    string PrimaryAudioCodec { get; set; }
    long PrimaryAudioBitRate { get; set; }

    // Primary subtitle track

    string PrimarySubtitleCodec { get; set; }
}