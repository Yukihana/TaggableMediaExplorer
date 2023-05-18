using System;
using System.Text.Json;

namespace TTX.Library.InstancingHelpers;

public static partial class CopyHelper
{
    public static T DeserializedCopy<T>(this T source)
        => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source))
        ?? throw new NullReferenceException();

    public static T DeserializedCopyAs<T>(this object source)
        => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source))
        ?? throw new NullReferenceException();

    // Add ReflectedCopy and ReflectedCopyAs, then remove the counterparts from ObjectHelper
}