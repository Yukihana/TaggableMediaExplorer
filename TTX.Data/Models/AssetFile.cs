using System;

namespace TTX.Data.Models;

/// <summary>
/// Asset file metadata.
/// </summary>
public class AssetFile
{
    public string FullPath { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.Now;
    public DateTime ModifiedUtc { get; set; } = DateTime.Now;
    public long SizeBytes { get; set; } = 0;
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();
    public byte[]? SHA256 { get; set; } = null;
}