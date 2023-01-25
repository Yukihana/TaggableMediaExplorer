using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;

namespace TTX.Client.Extensions;

internal static class QueryHelper
{
    public static string ToQuery<T>(this T poco)
    {
        List<KeyValuePair<string, string>> query = new();

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            if (property.GetType() is Type ptype && (
                ptype.IsPrimitive || ptype == typeof(string)) &&
                property.GetValue(poco)?.ToString() is string value)
            {
                query.Add(new(property.Name, value));
            }
        }

        return new FormUrlEncodedContent(query).ToString() ?? string.Empty;
    }
}