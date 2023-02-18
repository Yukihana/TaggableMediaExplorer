using System;
using System.IO.Enumeration;

namespace TTX.Library.FileSystemHelpers;

public static class PlatformNamingHelper
{
    private static readonly bool _platformIsCaseSensitive;
    private static readonly StringComparer _FilenameComparer;

    static PlatformNamingHelper()
    {
        _platformIsCaseSensitive = !IsPlatformCaseInsensitive();
        _FilenameComparer = _platformIsCaseSensitive
            ? StringComparer.FromComparison(StringComparison.Ordinal)
            : StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase);
    }

    public static bool PlatformIsCaseSensitive
        => _platformIsCaseSensitive;

    public static StringComparer FilenameComparer
        => _FilenameComparer;

    public static bool MatchByPattern(this ReadOnlySpan<char> name, ReadOnlySpan<char> expression)
        => FileSystemName.MatchesWin32Expression(expression, name, _platformIsCaseSensitive);

    private static bool IsPlatformCaseInsensitive() =>
        OperatingSystem.IsWindows() ||
        OperatingSystem.IsMacOS() ||
        OperatingSystem.IsIOS() ||
        OperatingSystem.IsTvOS() ||
        OperatingSystem.IsWatchOS();
}