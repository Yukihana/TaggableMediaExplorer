using System;
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
    /// Extract relevant properties and fields into a new instance of the required type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Obj"></param>
    /// <returns></returns>
    public static T CopyValues<T>(this object Obj) where T : new()
    {
        T result = new();

        foreach (var prop in Obj.GetType().GetProperties().Where(x => x.CanRead))
        {
            if (typeof(T).GetProperty(prop.Name) is PropertyInfo targetProp &&
                targetProp.CanWrite &&
                prop.PropertyType == targetProp.PropertyType)
            {
                targetProp.SetValue(result, prop.GetValue(Obj));
            }
        }

        // Fields after properties incase setters aren't identical.
        foreach (var field in Obj.GetType().GetFields())
        {
            if (typeof(T).GetField(field.Name) is FieldInfo targetField &&
                field.FieldType == targetField.FieldType)
            {
                targetField.SetValue(result, field.GetValue(Obj));
            }
        }

        return result.CopyFullyDecoupled();
    }

    /*
    internal static DbSet<AssetRecord> NullCheckAssetsTable(TTXContext? db)
    {
        if (db != null)
        {
            if (db.Assets != null)
            {
                return db.Assets;
            }
        }
        throw new NullReferenceException();
    }*/

    internal static void CopyFieldValuesTo<T>(this T source, T target) where T : class
    {
        foreach (var field in typeof(T).GetFields())
        {
            field.SetValue(target, field.GetValue(source));
        }
    }

    internal static void CopyPropertyValuesTo<T>(this T source, T target) where T : class
    {
        foreach (var prop in typeof(T).GetProperties())
        {
            if (prop.CanWrite && prop.CanRead)
            {
                prop.SetValue(target, prop.GetValue(source));
            }
        }
    }

    internal static int CopyValues(this object source, object target)
    {
        int count = 0;
        foreach (var prop in source.GetType().GetProperties().Where(x => x.CanRead))
        {
            if (target.GetType().GetProperty(prop.Name) is PropertyInfo targetProp)
            {
                if (targetProp.CanWrite)
                    targetProp.SetValue(target, prop.GetValue(source));
                count++;
            }
        }
        return count;
    }

    internal static int CopyPropertyValuesUnsafe(this object source, object target)
    {
        int count = 0;
        foreach (var prop in source.GetType().GetProperties().Where(x => x.CanRead))
        {
            if (target.GetType().GetProperty(prop.Name) is PropertyInfo targetProp)
            {
                if (targetProp.CanWrite)
                    targetProp.SetValue(target, prop.GetValue(source));
                count++;
            }
        }
        return count;
    }
}