using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TTX.Data.Extensions;
using TTX.Data.Models;
using TTX.Library.DataHelpers;

namespace TTX.Data.Comparers;

public class AssetIntegrityComparer : IEqualityComparer<IAssetHashedMetadata>
{
    public bool Equals(IAssetHashedMetadata? x, IAssetHashedMetadata? y)
    {
        if (x is null || y is null)
            return x == y;

        if (x.SizeBytes != y.SizeBytes)
            return false;

        if (!x.Crumbs.SequenceEqual(y.Crumbs))
            return false;

        return x.SHA256.SequenceEqual(y.SHA256);
    }

    public int GetHashCode([DisallowNull] IAssetHashedMetadata assetSyncInfo)
        => assetSyncInfo.GetIntegrityBytes().GetFNV1();
}