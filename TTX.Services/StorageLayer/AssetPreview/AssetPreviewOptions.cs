using System;
using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.StorageLayer.AssetPreview;

public class AssetPreviewOptions : IAssetPreviewOptions
{
    public string AssetsPath { get; set; } = "Assets";
    public string PreviewsPath { get; set; } = "Previews";
    public float AssetPreviewSnapshotTime { get; set; } = 0.2f;

    // Derived

    [JsonIgnore]
    public string AssetsPathFull { get; set; } = string.Empty;

    [JsonIgnore]
    public string PreviewsPathFull { get; set; } = string.Empty;

    // Init

    public void Initialize()
        => throw new NotImplementedException();

    public void Initialize(IRuntimeConfig runtimeConfig)
    {
        AssetsPathFull = Path.Combine(runtimeConfig.ProfileRoot, AssetsPath);
        PreviewsPathFull = Path.Combine(runtimeConfig.ProfileRoot, PreviewsPath);
    }
}