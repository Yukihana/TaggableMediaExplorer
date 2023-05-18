using System;
using System.ComponentModel.DataAnnotations;
using TTX.Data.ServerData.Models;

namespace TTX.Data.ServerData.Entities;

public class AssetRecord : IAssetItemId, IAssetFullSyncInfo, IMediaInfo
{
    [Key]
    public int ID { get; set; } = 0;

    // Identity

    public byte[] ItemId { get; set; } = Array.Empty<byte>();

    // Metadata

    public string LocalPath { get; set; } = string.Empty;
    public long SizeBytes { get; set; } = 0;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();

    // Hash Info

    public DateTime VerifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] SHA256 { get; set; } = Array.Empty<byte>();

    // Media Info

    public string MediaFormat { get; set; } = string.Empty;
    public TimeSpan MediaDuration { get; set; } = TimeSpan.Zero;

    // Media Info : Primary video track

    public string PrimaryVideoCodec { get; set; } = string.Empty;
    public int PrimaryVideoWidth { get; set; } = 0;
    public int PrimaryVideoHeight { get; set; } = 0;
    public long PrimaryVideoBitRate { get; set; } = 0;

    // Media Info : Primary audio track

    public string PrimaryAudioCodec { get; set; } = string.Empty;
    public long PrimaryAudioBitRate { get; set; } = 0;

    // Media Info : Primary subtitle track

    public string PrimarySubtitleCodec { get; set; } = string.Empty;

    // User Data

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public DateTime AddedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

    // Undecided

    public string SimilarIgnore { get; set; } = string.Empty;
}