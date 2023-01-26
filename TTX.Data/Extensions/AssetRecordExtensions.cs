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

    public static AssetRecord GenerateAssetRecord(this AssetFile file, string localPath) => new()
    {
        ItemId = Guid.NewGuid().ToByteArray(),

        LastLocation = localPath,
        SizeBytes = file.SizeBytes,
        Crumbs = file.Crumbs,
        SHA256 = file.SHA256 ?? throw new NullReferenceException($"Missing SHA256 when creating {nameof(AssetRecord)}"),

        CreatedUtc = file.CreatedUtc,
        ModifiedUtc = file.ModifiedUtc,

        Name = GetDefaultNameFromPath(localPath),
    };
}