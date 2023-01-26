using System;
using System.Collections.Generic;

namespace TTX.Data.Shared.QueryObjects;

public class AssetCardResponse
{
    public byte[] ItemId { get; set; } = Array.Empty<byte>();
    public string Name { get; set; } = string.Empty;

    // Metadata

    public HashSet<string> Tags { get; set; } = new();
    public DateTime AddedUtc { get; set; } = DateTime.UtcNow;
    public long SizeBytes { get; set; } = 0;

    // Codec Information

    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public uint Height { get; set; } = 0;
    public uint Width { get; set; } = 0;
}