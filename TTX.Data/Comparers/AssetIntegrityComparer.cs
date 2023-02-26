using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TTX.Data.Extensions;
using TTX.Data.Models;
using TTX.Library.DataHelpers;

namespace TTX.Data.Comparers;

// Not needed because we're using a concurrent dictionary to store identicals for logging
// Keep it around anyway, incase it can be used future.
public class AssetIntegrityComparer : IEqualityComparer<IAssetFullSyncInfo>
{
    public bool Equals(IAssetFullSyncInfo? x, IAssetFullSyncInfo? y)
    {
        if (x is null || y is null)
            return x == y;

        if (x.SizeBytes != y.SizeBytes)
            return false;

        if (!x.Crumbs.SequenceEqual(y.Crumbs))
            return false;

        return x.SHA256.SequenceEqual(y.SHA256);
    }

    public int GetHashCode([DisallowNull] IAssetFullSyncInfo assetSyncInfo)
        => assetSyncInfo.GetIntegrityBytes().GetFNV1();
}