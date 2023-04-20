namespace TTX.Services.StorageLayer.AssetPreview;

public interface IAssetPreviewOptions : IServiceProfile
{
    string AssetsPath { get; set; }
    string PreviewsPath { get; set; }
    float AssetPreviewSnapshotTime { get; set; }
}