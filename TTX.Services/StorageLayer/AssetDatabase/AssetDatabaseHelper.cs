using System;
using System.Globalization;
using System.IO;
using TTX.Data.Entities;
using TTX.Data.Models;
using TTX.Library.InstancingHelpers;

namespace TTX.Services.StorageLayer.AssetDatabase;

internal static partial class AssetDatabaseHelper
{
    public static string GetDefaultNameFromPath(this string path)
    {
        string rawname = Path.GetFileNameWithoutExtension(path);
        TextInfo ti = new CultureInfo("en-GB", false).TextInfo;
        string sanitized = rawname
            .Replace('-', ' ')
            .Replace('_', ' ');
        return ti.ToTitleCase(sanitized);
    }

    public static AssetRecord GenerateRecord(this IAssetFullSyncInfo syncInfo, AssetMediaInfo mediaInfo)
    {
        AssetRecord rec = new()
        {
            ItemId = Guid.NewGuid().ToByteArray(),

            LocalPath = syncInfo.LocalPath,
            SizeBytes = syncInfo.SizeBytes,
            Crumbs = syncInfo.Crumbs,
            SHA256 = syncInfo.SHA256,

            CreatedUtc = syncInfo.CreatedUtc,
            ModifiedUtc = syncInfo.ModifiedUtc,

            Title = syncInfo.LocalPath.GetDefaultNameFromPath(),
        };

        // Copy media info
        mediaInfo.CopyConstrainedTo<IMediaInfo>(rec);

        return rec;
    }
}