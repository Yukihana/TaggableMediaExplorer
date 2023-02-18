using System;
using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.IncomingLayer.AssetTracking;

internal class AssetTrackingOptions : IAssetTrackingOptions
{
    // Base options

    public string ServerRoot { get; set; } = string.Empty;
    public string AssetsPath { get; set; } = "Assets";
    public string[] Whitelist { get; set; } = { "*.*" };
    public string[] Blacklist { get; set; } = Array.Empty<string>();
    public string[] FinalAdds { get; set; } = Array.Empty<string>();

    // Derived

    [JsonIgnore]
    public string AssetsPathFull { get; set; } = string.Empty;

    public void Initialize()
    {
        AssetsPathFull = Path.Combine(ServerRoot, AssetsPath);
    }
}