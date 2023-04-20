using System;
using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.ApiLayer.AssetContent;

internal class AssetContentOptions : IAssetContentOptions
{
    public string AssetsPath { get; set; } = "Assets";

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