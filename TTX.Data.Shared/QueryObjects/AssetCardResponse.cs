using System;
using System.Collections.Generic;

namespace TTX.Data.Shared.QueryObjects;

public class AssetCardResponse
{
    public byte[] ItemId { get; set; } = Array.Empty<byte>();

    // Metadata

    public long SizeBytes { get; set; } = 0;

    // Codec Information

    public TimeSpan MediaDuration { get; set; } = TimeSpan.Zero;
    public int PrimaryVideoWidth { get; set; } = 0;
    public int PrimaryVideoHeight { get; set; } = 0;

    // User Data

    public string Title { get; set; } = string.Empty;
    public HashSet<string> Tags { get; set; } = new();
    public DateTime AddedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
}