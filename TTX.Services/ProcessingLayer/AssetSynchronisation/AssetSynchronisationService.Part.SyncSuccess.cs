using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService
{
    private async partial Task OnSyncSuccess(AssetRecord asset, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        await _assetPreview.Validate(asset, ctoken).ConfigureAwait(false);

        _assetPresence.Set(asset.LocalPath, asset.ItemId);
    }
}