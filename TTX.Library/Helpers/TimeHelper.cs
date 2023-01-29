using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTX.Library.Helpers;

public static class TimeHelper
{
    private static readonly string[] dtype = { "Added", "Added on", "Updated", "Updated on" };
    private static readonly string[] verbs = { "just now", "minutes ago", "an hour ago", "hours ago", "yesterday", "days ago", "in the future", "unknown" };
    private static readonly string months = "JanFebMarAprMayJunJulAugSepOctNovDec";

    public static string GetTimeDiff(this DateTime since, bool IsAddedDate = false)
    {
        DateTime dtNow = DateTime.Now;
        var t = TimeSpan.FromTicks(dtNow.Ticks - since.Ticks);

        var m = t.TotalMinutes;
        var h = t.TotalHours;
        var d = t.TotalDays;
        var yearstart = new DateTime(dtNow.Year, 1, 1, 0, 0, 0);

        // Just now (5mins)
        if (m < 5)
        {
            return $"{dtype[IsAddedDate ? 0 : 2]} {verbs[0]}";
        }
        // X minutes ago (5~59mins)
        else if (m < 60)
        {
            return $"{dtype[IsAddedDate ? 0 : 2]} {Math.Floor(m)} {verbs[1]}";
        }
        // An hour ago (60~119mins)
        else if (m < 120)
        {
            return $"{dtype[IsAddedDate ? 0 : 2]} {verbs[2]}";
        }
        // X hours ago (2~23 hours)
        else if (h < 24)
        {
            return $"{dtype[IsAddedDate ? 0 : 2]} {Math.Floor(h)} {verbs[3]}";
        }
        // Yesterday (24~47 hours)
        else if (h < 48)
        {
            return $"{dtype[IsAddedDate ? 0 : 2]} {verbs[4]}";
        }
        // X days ago (2 days to a week)
        else if (h < 24 * 7)
        {
            return $"{dtype[IsAddedDate ? 0 : 2]} {Math.Floor(d)} {verbs[5]}";
        }
        // X <month> (Up until previous year)
        else if (since >= yearstart)
        {
            return $"{dtype[IsAddedDate ? 1 : 3]} {since.Day} {months.Substring(3 * (since.Month - 1), 3)}";
        }
        // <month> <year> (beyond that)
        else if (since < yearstart)
        {
            return $"{dtype[IsAddedDate ? 1 : 3]} {months.Substring(3 * (since.Month - 1), 3)}, {since.Year}";
        }
        // In the future
        else if (since > dtNow)
        {
            return $"{dtype[IsAddedDate ? 0 : 2]} {verbs[6]} LOL";
        }
        // Error or Zero
        return $"{dtype[IsAddedDate ? 0 : 2]} : {verbs[7]}";
    }

    public static string ToConciseDuration(this TimeSpan duration)
        => string.Format(duration.TotalHours < 1 ? @"{0:m\:ss}" : @"{0\:h\:mm\:ss", duration);
}