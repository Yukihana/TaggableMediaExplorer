using System;

namespace TTX.Library.ObjectHelpers;

public static class NullHelper
{
    public static T ThrowOnReferenceNull<T>(this T? data, string message)
        => data ?? throw new NullReferenceException(message);
}