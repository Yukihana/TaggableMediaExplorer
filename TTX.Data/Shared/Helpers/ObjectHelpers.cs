using System;
using System.Text.Json;

namespace TTX.Data.Shared.Helpers;

public static class ObjectHelpers
{
    /// <summary>
    /// Makes a deep copy using the Json Serializer
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="source">The object to be copied.</param>
    /// <returns>A fully decoupled true deep copy.</returns>
    /// <exception cref="NullReferenceException">Returned when the copy operation fails.</exception>
    public static T MakeDeepCopy<T>(this T source)
        => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source))
        ?? throw new NullReferenceException();

    public static T Extract<T>(this object Obj) where T : struct
    {
        // TODO
        return new T()
        {
        };
    }
}