using System;

namespace TTX.Data.Models;

/// <summary>
/// Asset synchronisation information with quick-sync metadata.
/// </summary>
public class QuickAssetSyncInfo : AssetSyncInfoBase, IAssetMetadata
{
    public string LocalPath { get; set; } = string.Empty;
    public long SizeBytes { get; set; } = 0;
    public DateTime CreatedUtc { get; set; } = DateTime.Now;
    public DateTime ModifiedUtc { get; set; } = DateTime.Now;
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();
}