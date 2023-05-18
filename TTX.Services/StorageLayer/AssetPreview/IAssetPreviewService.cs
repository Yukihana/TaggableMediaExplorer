using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData.Entities;

namespace TTX.Services.StorageLayer.AssetPreview;

public interface IAssetPreviewService
{
    // Management

    Task Validate(AssetRecord asset, CancellationToken ctoken = default);

    // Api

    string? GetSnapshotPath(string idString);
}