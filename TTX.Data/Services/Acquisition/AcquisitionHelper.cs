using System.Collections.Generic;
using System.IO;

namespace TTX.Data.Services.Acquisition;

public static class AcquisitionHelper
{
    public static AcquisitionOptions ExtractOptions(object profile)
    {
        return (AcquisitionOptions)profile;
    }

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