using System;
using System.Linq;
using TTX.Data.Models;
using TTX.Library.FileSystemHelpers;

namespace TTX.Data.Extensions;

public static partial class AssetRecordExtensions
{
    public static bool QuickSyncEquals(this IAssetMetadata compareSource, IAssetMetadata compareTarget)
    {
        if (!compareSource.LocalPath.Equals(compareTarget.LocalPath, PlatformNamingHelper.FilenameComparison))
            return false;
        if (compareSource.SizeBytes != compareTarget.SizeBytes)
            return false;
        if (compareSource.ModifiedUtc != compareTarget.ModifiedUtc)
            return false;
        return compareSource.Crumbs.SequenceEqual(compareTarget.Crumbs);
    }

    public static bool IntegrityEquals(this IAssetHashedMetadata compareSource, IAssetHashedMetadata compareTarget)
    {
        if (compareSource.SizeBytes != compareTarget.SizeBytes)
            return false;
        if (!compareSource.Crumbs.SequenceEqual(compareTarget.Crumbs))
            return false;
        return compareSource.SHA256.SequenceEqual(compareTarget.SHA256);
    }
}