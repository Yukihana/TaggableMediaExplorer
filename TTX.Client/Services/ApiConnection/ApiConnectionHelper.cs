using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using TTX.Library.Configurations;

namespace TTX.Client.Services.ApiConnection;

internal static class ApiConnectionHelper
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

    public static StringContent ToStringContent<T>(this T requestBody)
    {
        string json = JsonSerializer.Serialize(requestBody, JsonHelper.ApiEgressOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}