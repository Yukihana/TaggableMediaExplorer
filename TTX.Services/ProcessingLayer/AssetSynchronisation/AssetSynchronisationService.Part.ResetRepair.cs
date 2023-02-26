using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public partial class AssetSynchronisationService
{
    /// <summary>
    /// Clears current presences and scans the database for conflicts.
    /// </summary>
    public async partial Task ResetRepair(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        _assetPresence.Clear();
        await _assetDatabase.ScanRepairAnalyse(ctoken).ConfigureAwait(false);
    }
}