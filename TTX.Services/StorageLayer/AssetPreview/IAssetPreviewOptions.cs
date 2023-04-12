namespace TTX.Services.StorageLayer.AssetPreview;

public interface IAssetPreviewOptions : IServiceOptions
{
    string ServerRoot { get; set; }
    string AssetsPath { get; set; }
    string PreviewsPath { get; set; }
    float AssetPreviewSnapshotTime { get; set; }
}