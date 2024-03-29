﻿using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace TTX.Library.Helpers;

public static class ObjectHelpers
{
    /// <summary>
    /// Makes a deep copy using the Json Serializer
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="source">The object to be copied.</param>
    /// <returns>A fully decoupled true deep copy.</returns>
    /// <exception cref="NullReferenceException">Returned when the copy operation fails.</exception>
    public static T CopyFullyDecoupled<T>(this T source)
        => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source))
        ?? throw new NullReferenceException();

    /// <summary>
    /// Makes a deep copy using the Json Serializer
    /// </summary>
    /// <typeparam name="TIn">The object type.</typeparam>
    /// <typeparam name="TOut">The object type to be copied to.</typeparam>
    /// <param name="source">The object to be copied.</param>
    /// <returns>A fully decoupled true deep copy.</returns>
    /// <exception cref="NullReferenceException">Returned when the copy operation fails.</exception>
    public static TOut CopyBySerialization<TIn, TOut>(this TIn source)
        => JsonSerializer.Deserialize<TOut>(JsonSerializer.Serialize(source), new JsonSerializerOptions() { })
        ?? throw new NullReferenceException();

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T CopyByReflection<T>(this object source) where T : new()
    {
        T result = new();

        foreach (var field in source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            if (typeof(T).GetField(field.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is FieldInfo targetField &&
                field.FieldType == targetField.FieldType)
            {
                targetField.SetValue(result, field.GetValue(source));
            }
        }

        return result.CopyFullyDecoupled();
    }

    /// <summary>
    /// Extract relevant properties and fields into a new instance of the required type.
    /// </summary>
    /// <typeparam name="T">The output type</typeparam>
    /// <param name="source">The object to copy values from</param>
    /// <param name="copyFields">If set to true, fields will also be copied after properties</param>
    /// <returns></returns>
    public static T CopyValues<T>(this object source, bool copyFields = true) where T : new()
    {
        T result = new();

        foreach (var prop in source.GetType().GetProperties().Where(x => x.CanRead))
        {
            if (typeof(T).GetProperty(prop.Name) is PropertyInfo targetProp &&
                targetProp.CanWrite &&
                prop.PropertyType == targetProp.PropertyType)
            {
                targetProp.SetValue(result, prop.GetValue(source));
            }
        }

        if (copyFields)
        {
            // Fields after properties incase setters aren't identical.
            foreach (var field in source.GetType().GetFields())
            {
                if (typeof(T).GetField(field.Name) is FieldInfo targetField &&
                    field.FieldType == targetField.FieldType)
                {
                    targetField.SetValue(result, field.GetValue(source));
                }
            }
        }

        return result.CopyFullyDecoupled();
    }
}