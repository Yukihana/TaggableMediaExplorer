using System.Text;
using System.Text.Json;

namespace TTX.Library.Helpers;

public static class EncodeHelper
{
    private const string DIGITS = "0123456789ABCDEF";

    public static string ToHex(this byte[] bytes)
    {
        StringBuilder result = new();

        for (int i = 0; i < bytes.Length; i++)
        {
            // Use bitwise operators here instead
            result.Append(DIGITS[bytes[i] / 16]);
            result.Append(DIGITS[bytes[i] % 16]);
        }

        return result.ToString();
    }

    public static T? DeserializeJsonResponse<T>(this string json)
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        try { return JsonSerializer.Deserialize<T>(json, options); }
        catch { return default; }
    }
}