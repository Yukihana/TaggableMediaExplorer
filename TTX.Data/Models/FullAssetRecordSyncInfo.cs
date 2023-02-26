using System;

namespace TTX.Data.Models;

/// <summary>
/// Asset synchronisation information with quick-sync and hash metadata.
/// </summary>
public class FullAssetRecordSyncInfo : FullAssetSyncInfo, IAssetItemId
{
    public byte[] ItemId { get; set; } = Array.Empty<byte>();
}