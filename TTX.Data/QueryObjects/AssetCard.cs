using System;

namespace TTX.Data.QueryObjects;

public class AssetCard
{
    public string GUID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;

    // Metadata

    public TimeSpan AddedUtc { get; set; } = TimeSpan.Zero;
    public long SizeBytes { get; set; } = 0;

    // Codec Information

    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public uint VideoHeight { get; set; } = 0;
    public uint VideoWidth { get; set; } = 0;
}