using System.Text;

namespace TTX.Data.Encoding;

internal static class BytesHex
{
    private const string DIGITS = "0123456789ABCDEF";

    public static string ToHex(this byte[] bytes)
    {
        StringBuilder result = new();

        for (int i = 0; i < bytes.Length; i++)
        {
            result.Append(DIGITS[bytes[i] / 16]);
            result.Append(DIGITS[bytes[i] % 16]);
        }

        return result.ToString();
    }
}