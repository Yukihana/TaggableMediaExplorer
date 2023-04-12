using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.StorageLayer.AssetPreview;

public class AssetPreviewOptions : IAssetPreviewOptions
{
    public string ServerRoot { get; set; } = string.Empty;
    public string AssetsPath { get; set; } = "Assets";
    public string PreviewsPath { get; set; } = "Previews";
    public float AssetPreviewSnapshotTime { get; set; } = 0.2f;

    // Derived

    [JsonIgnore]
    public string AssetsPathFull { get; set; } = string.Empty;

    [JsonIgnore]
    public string PreviewsPathFull { get; set; } = string.Empty;

    public void Initialize()
    {
        AssetsPathFull = Path.Combine(ServerRoot, AssetsPath);
        PreviewsPathFull = Path.Combine(ServerRoot, PreviewsPath);
    }
}