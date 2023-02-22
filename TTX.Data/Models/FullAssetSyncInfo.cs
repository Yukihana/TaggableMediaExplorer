using System;

namespace TTX.Data.Models;

/// <summary>
/// Asset synchronisation information with quick-sync and hash metadata.
/// </summary>
public class FullAssetSyncInfo : QuickAssetSyncInfo, IAssetHashInfo
{
    public byte[] SHA256 { get; set; } = Array.Empty<byte>();
    public DateTime VerifiedUtc { get; set; } = DateTime.Now;
}