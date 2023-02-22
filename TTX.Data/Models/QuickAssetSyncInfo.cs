using System;

namespace TTX.Data.Models;

/// <summary>
/// Asset synchronisation information with quick-sync metadata.
/// </summary>
public class QuickAssetSyncInfo : IAssetMetadata
{
    public string FullPath { get; set; } = string.Empty;
    public string LocalPath { get; set; } = string.Empty;

    // Metadata

    public DateTime CreatedUtc { get; set; } = DateTime.Now;
    public DateTime ModifiedUtc { get; set; } = DateTime.Now;
    public long SizeBytes { get; set; } = 0;
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();
}