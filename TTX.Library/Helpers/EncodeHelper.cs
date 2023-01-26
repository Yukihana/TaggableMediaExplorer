using System;
using System.Text.Json;

namespace TTX.Library.Helpers;

public static class EncodeHelper
{
    public static T? DeserializeJsonResponse<T>(this string json)
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        // figure out camelCase to pascal case.
        // because it uses JsonSerializerDefaults.Web
        try { return JsonSerializer.Deserialize<T>(json, options); }
        catch { return default; }
    }
}