using System;

namespace TTX.Data.Models;

/// <summary>
/// Asset file metadata with hash information.
/// </summary>
public class HashedAssetFile : AssetFile
{
    public byte[] SHA2 { get; set; } = Array.Empty<byte>();
}