using System.Text.RegularExpressions;

namespace TTX.Library.Helpers.StringHelpers;

public static partial class RegexHelpers
{
    [GeneratedRegex("[^a-zA-Z0-9]+")]
    public static partial Regex EnglishAlphanumerics();
}