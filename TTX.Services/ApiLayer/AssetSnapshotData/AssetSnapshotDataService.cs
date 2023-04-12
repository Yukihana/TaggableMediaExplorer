using TTX.Services.StorageLayer.AssetPreview;

namespace TTX.Services.ApiLayer.AssetSnapshotData;

public class AssetSnapshotDataService : IAssetSnapshotDataService
{
    private readonly IAssetPreviewService _assetPreview;

    public AssetSnapshotDataService(IAssetPreviewService assetPreview)
    {
        _assetPreview = assetPreview;
    }

    public string? GetPath(string id)
        => _assetPreview.GetSnapshotPath(id);
}