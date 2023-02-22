using System;
using System.Globalization;
using System.IO;
using TTX.Data.Encoding;
using TTX.Data.Entities;
using TTX.Data.Models;

namespace TTX.Data.Extensions;

public static partial class AssetRecordExtensions
{
    // String
    public static string ToIdentityString(this AssetRecord rec)
    {
        try
        {
            rec.Lock.EnterReadLock();
            return
                rec.SizeBytes.ToString() + "_" +
                rec.Crumbs.ToHex() + "_" +
                rec.SHA256.ToHex();
        }
        finally { rec.Lock.ExitReadLock(); }
    }

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

        FilePath = info.LocalPath,
        SizeBytes = info.SizeBytes,
        Crumbs = info.Crumbs,
        SHA256 = info.SHA256,

        CreatedUtc = info.CreatedUtc,
        ModifiedUtc = info.ModifiedUtc,

        Title = GetDefaultNameFromPath(info.LocalPath),
    };
}