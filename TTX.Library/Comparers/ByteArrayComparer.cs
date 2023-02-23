using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
    {
        // Implement FNV1_32
        unchecked
        {
            const int prime = (int)0x01000193;
            int hash = (int)0x811c9dc5;

            for (int i = 0; i < obj.Length; i++)
            {
                hash *= prime;
                hash ^= obj[i];
            }

            return hash;
        }
    }
}