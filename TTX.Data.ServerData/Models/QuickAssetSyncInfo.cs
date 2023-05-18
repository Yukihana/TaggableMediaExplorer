using System;

namespace TTX.Data.ServerData.Models;

/// <summary>
/// Asset synchronisation information with quick-sync metadata.
/// </summary>
public class QuickAssetSyncInfo : AssetSyncInfoBase, IAssetQuickSyncInfo
{
    public string LocalPath { get; set; } = string.Empty;
    public long SizeBytes { get; set; } = 0;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();
}