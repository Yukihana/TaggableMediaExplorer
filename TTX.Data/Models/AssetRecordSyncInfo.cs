using System;

namespace TTX.Data.Models;

public class AssetRecordSyncInfo : IAssetItemId, IAssetFullSyncInfo
{
    public byte[] ItemId { get; set; } = Array.Empty<byte>();
    public string LocalPath { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;
    public long SizeBytes { get; set; } = 0;
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();
    public byte[] SHA256 { get; set; } = Array.Empty<byte>();
    public DateTime VerifiedUtc { get; set; } = DateTime.UtcNow;
}