using System;
using System.Linq;
using TTX.Data.ServerData.Models;
using TTX.Library.FileSystemHelpers;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

internal static partial class AssetSynchronisationHelper
{
    internal static bool ProvisionallyEquals(this IAssetQuickSyncInfo source, IAssetQuickSyncInfo target)
    {
        if (!source.LocalPath.Equals(target.LocalPath, PlatformNamingHelper.FilenameComparison))
            return false;
        if (source.SizeBytes != target.SizeBytes)
            return false;
        if (source.ModifiedUtc != target.ModifiedUtc)
            return false;
        return source.Crumbs.SequenceEqual(target.Crumbs);
    }
}