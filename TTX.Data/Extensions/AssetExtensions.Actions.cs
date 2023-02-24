using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TTX.Data.Entities;
using TTX.Data.Models;
using TTX.Library.Helpers;

namespace TTX.Data.Extensions;

public static partial class AssetRecordExtensions
{
    public static byte[] GetIntegrityBytes(this IAssetHashedMetadata hashedMetadata)
    {
        // TODO_Optimization
        List<byte> bytes = new();
        bytes.AddRange(BitConverter.GetBytes(hashedMetadata.SizeBytes));
        bytes.AddRange(hashedMetadata.Crumbs);
        bytes.AddRange(hashedMetadata.SHA256);

        return bytes.ToArray();
    }

    // String
    public static string ToIdentityString(this IAssetHashedMetadata hashedMetadata)
        => hashedMetadata.GetIntegrityBytes().ToHex();

    public static string GetDefaultNameFromPath(this string path)
    {
        string rawname = Path.GetFileNameWithoutExtension(path);
        TextInfo ti = new CultureInfo("en-GB", false).TextInfo;
        string sanitized = rawname
            .Replace('-', ' ')
            .Replace('_', ' ');
        return ti.ToTitleCase(sanitized);
    }

    public static AssetRecord GenerateRecord(this FullAssetSyncInfo info) => new()
    {
        ItemId = Guid.NewGuid().ToByteArray(),

        LocalPath = info.LocalPath,
        SizeBytes = info.SizeBytes,
        Crumbs = info.Crumbs,
        SHA256 = info.SHA256,

        CreatedUtc = info.CreatedUtc,
        ModifiedUtc = info.ModifiedUtc,

        Title = GetDefaultNameFromPath(info.LocalPath),
    };
}