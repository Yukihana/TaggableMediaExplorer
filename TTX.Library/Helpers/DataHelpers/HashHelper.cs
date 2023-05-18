using System.Diagnostics.CodeAnalysis;

namespace TTX.Library.DataHelpers;

public static class HashHelper
{
    public static int GetFNV1([DisallowNull] this byte[] bytes)
    {
        unchecked
        {
            const int prime = (int)0x01000193;
            int hash = (int)0x811c9dc5;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash *= prime;
                hash ^= bytes[i];
            }

            return hash;
        }
    }
}