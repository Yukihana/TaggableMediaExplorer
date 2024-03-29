﻿using System.Globalization;

namespace TTX.Library.Helpers.StringHelpers;

public static partial class IdNamingHelper
{
    private static TextInfo? _textInfo = null;

    public static TextInfo GetTextInfo()
    {
        if (_textInfo != null)
            return _textInfo;
        var textInfo = new CultureInfo("en-GB", false).TextInfo;
        _textInfo = textInfo;
        return textInfo;
    }

    public static string ToTitleFormat(this string id)
    {
        string spacedName = id
            .Replace('_', ' ')
            .Replace('-', ' ');

        return GetTextInfo().ToTitleCase(spacedName);
    }

    public static string ToTagFormat(this string id)
    {
        return RegexHelpers
            .EnglishAlphanumerics()
            .Replace(id, "-")
            .ToLowerInvariant()
            .Trim('-');
    }
}