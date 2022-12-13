using System;

namespace TTX.Data.Services.Acquisition;

public class AcquisitionOptions
{
    public string AssetsPath { get; set; } = "Assets";
    public string AssetsPathFull { get; set; } = string.Empty;
    public string[] Whitelist { get; set; } = { "*.*" };
    public string[] Blacklist { get; set; } = Array.Empty<string>();
    public string[] FinalAdds { get; set; } = Array.Empty<string>();
}