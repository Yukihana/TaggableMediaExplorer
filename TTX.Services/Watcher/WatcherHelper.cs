using System.Collections.Generic;
using System.IO;

namespace TTX.Services.Watcher;

public static class WatcherHelper
{
    public static HashSet<string> GetLocalFilePathsByPatterns(this string directory, IEnumerable<string> patterns)
    {
        HashSet<string> paths = new();
        foreach (var pattern in patterns)
        {
            var files = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                paths.Add(file);
            }
        }

        // Localize paths
        return paths;
    }
}