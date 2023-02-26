using System.IO;
using System.Linq;
using System.Threading;
using TTX.Library.FileSystemHelpers;

namespace TTX.Services.IncomingLayer.AssetTracking;

public partial class AssetTrackingService
{
    public partial string[] GetAllFiles(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        return Directory
            .GetFiles(_options.AssetsPathFull, "*.*", SearchOption.AllDirectories)
            .AsParallel().AsOrdered()
            .Where(ValidatePathByPattern)
            .Distinct(PlatformNamingHelper.FilenameComparer)
            .ToArray();
    }
}