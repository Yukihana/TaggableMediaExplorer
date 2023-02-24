using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TTX.Library.DataHelpers;

namespace TTX.Library.Comparers;

public class ByteArrayComparer : IEqualityComparer<byte[]>
{
    public bool Equals(byte[]? x, byte[]? y)
    {
        if (x is null || y is null)
            return x == y;

        if (x.Length != y.Length)
            return false;

        return x.SequenceEqual(y);
    }

    public int GetHashCode([DisallowNull] byte[] obj)
        => obj.GetFNV1();
}