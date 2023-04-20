using System;
using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.IncomingLayer.AssetTracking;

internal class AssetTrackingOptions : IAssetTrackingOptions
{
    // Base options

    public string AssetsPath { get; set; } = "Assets";
    public string[] Whitelist { get; set; } = { "*.*" };
    public string[] Blacklist { get; set; } = Array.Empty<string>();
    public string[] FinalAdds { get; set; } = Array.Empty<string>();

    // Derived

    [JsonIgnore]
    public string AssetsPathFull { get; set; } = string.Empty;

    // Init

    public void Initialize()
        => throw new NotImplementedException();

    public void Initialize(IRuntimeConfig runtimeConfig)
    {
        AssetsPathFull = Path.Combine(runtimeConfig.ProfileRoot, AssetsPath);
    }
}