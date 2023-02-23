using System;
using System.Collections.Generic;
using System.Linq;
using TTX.Library.Comparers;

namespace TTX.Library.DataHelpers;

public static class IdHelper
{
    public static byte[] GenerateUniqueGuid(this IEnumerable<byte[]> existing)
    {
        if (existing is not HashSet<byte[]> existingHashSet
            || existingHashSet.Comparer is not ByteArrayComparer)
            existingHashSet = existing.ToHashSet(new ByteArrayComparer());

        byte[] newGuid;
        do { newGuid = Guid.NewGuid().ToByteArray(); }
        while (!existingHashSet.Add(newGuid));
        return newGuid;
    }
}