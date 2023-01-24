using System;
using System.Collections.Generic;

namespace TTX.Data.Shared.QueryObjects;

public class AssetCard
{
    public byte[] GUID { get; set; } = Array.Empty<byte>();
    public string GuidString { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    // Metadata

    public HashSet<string> Tags { get; set; } = new();
    public TimeSpan AddedUtc { get; set; } = TimeSpan.Zero;
    public long SizeBytes { get; set; } = 0;

    // Codec Information

    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public uint Height { get; set; } = 0;
    public uint Width { get; set; } = 0;
}