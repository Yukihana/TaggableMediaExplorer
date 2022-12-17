using System;
using System.IO;

namespace TTX.Data.Services.Acquisition;

public class AcquisitionOptions
{
    public void Initialize()
    {
        AssetsPathFull = Path.Combine(ServerRoot, AssetsPath);
    }

    // Base options

    public string ServerRoot { get; set; } = string.Empty;
    public string AssetsPath { get; set; } = "Assets";
    public string[] Whitelist { get; set; } = { "*.*" };
    public string[] Blacklist { get; set; } = Array.Empty<string>();
    public string[] FinalAdds { get; set; } = Array.Empty<string>();

    // Derived

    public string AssetsPathFull { get; set; } = string.Empty;
}