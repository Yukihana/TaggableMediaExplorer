using System;
using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.Acquisition;

public class AcquisitionOptions : IAcquisitionOptions
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

    // Service IDs

    public string AcquisitionSID { get; set; } = "acq";
    public string MetadataSID { get; set; } = "mtd";

    // Derived

    [JsonIgnore]
    public string AssetsPathFull { get; set; } = string.Empty;
}