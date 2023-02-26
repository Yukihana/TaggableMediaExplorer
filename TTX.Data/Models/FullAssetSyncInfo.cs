using System;

namespace TTX.Data.Models;

/// <summary>
/// Asset synchronisation information with quick-sync and hash metadata.
/// </summary>
public class FullAssetSyncInfo : AssetSyncInfoBase, IAssetFullSyncInfo
{
    public string LocalPath { get; set; } = string.Empty;
    public long SizeBytes { get; set; } = 0;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();

    public byte[] SHA256 { get; set; } = Array.Empty<byte>();
    public DateTime VerifiedUtc { get; set; } = DateTime.UtcNow;
}