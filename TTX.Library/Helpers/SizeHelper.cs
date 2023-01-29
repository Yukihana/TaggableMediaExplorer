using System;

namespace TTX.Library.Helpers;

public static class SizeHelper
{
    private static readonly string[] sizetext
           = { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

    public static string ToSizeString(this long size)
    {
        double z = size;
        int n = 0;

        while (z >= 1024 && n <= sizetext.Length - 2)
        {
            n++;
            z = size / Math.Pow(1024, n);
        };

        if (n == 0)
            return size + " Bytes";
        else if (z < 100)
            return $"{Math.Round(z, 2)} {sizetext[n]}";
        else if (z < 1000)
            return $"{Math.Round(z, 1)} {sizetext[n]}";
        else
            return $"{Math.Floor(z)} {sizetext[n]}";
    }
}