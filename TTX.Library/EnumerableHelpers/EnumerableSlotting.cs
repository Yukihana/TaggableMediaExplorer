using System;
using System.Collections.Generic;
using System.Linq;

namespace TTX.Library.EnumerableHelpers;

public static class EnumerableSlotting
{
    public static T SelectOneOrThrow<T>(this IEnumerable<T> input, string exceptionMessagePrefix = "")
    {
        int count = input.Count();

        if (count < 1)
            throw new ArgumentException($"{exceptionMessagePrefix}Too few items({count}).");
        if (count > 1)
            throw new ArgumentException($"{exceptionMessagePrefix}Too many items({count}).");
        return input.First();
    }

    public static T? SelectOneNoneOrThrow<T>(this IEnumerable<T> input, string exceptionMessagePrefix = "")
    {
        int count = input.Count();

        if (count < 1)
            return default;
        if (count > 1)
            throw new ArgumentException($"{exceptionMessagePrefix}Too many items({count}).");
        return input.First();
    }
}