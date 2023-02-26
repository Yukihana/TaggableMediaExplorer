using System;
using System.Globalization;
using System.IO;
using TTX.Data.Entities;
using TTX.Data.Models;

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

    public static AssetRecord GenerateRecord(this IAssetFullSyncInfo info) => new()
    {
        ItemId = Guid.NewGuid().ToByteArray(),

        LocalPath = info.LocalPath,
        SizeBytes = info.SizeBytes,
        Crumbs = info.Crumbs,
        SHA256 = info.SHA256,

        CreatedUtc = info.CreatedUtc,
        ModifiedUtc = info.ModifiedUtc,

        Title = info.LocalPath.GetDefaultNameFromPath(),
    };
}