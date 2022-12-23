using System;

namespace TTX.Data.Messages;

/// <summary>
/// Asset file metadata.
/// </summary>
public struct AssetFile : IMessage
{
    public string TargetSID { get; set; }
    public string FullPath { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }
    public long SizeBytes { get; set; }
}