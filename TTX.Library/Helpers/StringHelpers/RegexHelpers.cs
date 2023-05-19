using System.Text.RegularExpressions;

namespace TTX.Library.Helpers.StringHelpers;

public static partial class RegexHelpers
{
    [GeneratedRegex("^[^\\w]+|[^\\w]+$")]
    public static partial Regex TrimPunctuations();

    [GeneratedRegex("[^a-zA-Z0-9]+")]
    public static partial Regex EnglishAlphanumerics();
}