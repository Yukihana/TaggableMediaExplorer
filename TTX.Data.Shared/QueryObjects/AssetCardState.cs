using System;

namespace TTX.Data.Shared.QueryObjects;

public class AssetCardState
{
    public string ItemId { get; set; } = string.Empty;

    // Metadata

    public long SizeBytes { get; set; } = 0;

    // Codec Information

    public TimeSpan MediaDuration { get; set; } = TimeSpan.Zero;
    public int PrimaryVideoWidth { get; set; } = 0;
    public int PrimaryVideoHeight { get; set; } = 0;

    // User Data

    public string Title { get; set; } = string.Empty;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public DateTime AddedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
}