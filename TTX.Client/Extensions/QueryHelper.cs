using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TTX.Client.Extensions;

internal static class QueryHelper
{
    public static string ToQuery<T>(this T poco)
    {
        List<KeyValuePair<string, string>> query = new();

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            if (property.PropertyType is Type ptype &&
                (ptype.IsPrimitive || ptype == typeof(string)) &&
                property.GetValue(poco)?.ToString() is string value &&
                !string.IsNullOrEmpty(value))
            {
                query.Add(new(property.Name, value));
            }
        }

        return new QueryBuilder(query).ToQueryString().ToString();
    }
}