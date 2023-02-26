using System;
using System.Collections.Generic;
using TTX.Data.Models;

namespace TTX.Data.Extensions;

public static partial class AssetRecordExtensions
{
    public static byte[] GetIntegrityBytes(this IAssetFullSyncInfo hashedMetadata)
    {
        // TODO_Optimization
        List<byte> bytes = new();
        bytes.AddRange(BitConverter.GetBytes(hashedMetadata.SizeBytes));
        bytes.AddRange(hashedMetadata.Crumbs);
        bytes.AddRange(hashedMetadata.SHA256);

        return bytes.ToArray();
    }
}