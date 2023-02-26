using System;

namespace TTX.Data.Models;

/// <summary>
/// Asset synchronisation information with quick-sync metadata.
/// </summary>
public class QuickAssetRecordSyncInfo : QuickAssetSyncInfo, IAssetItemId
{
    public byte[] ItemId { get; set; } = Array.Empty<byte>();
}