using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace TTX.Services.IncomingLayer.AssetTracking;

public partial class AssetTrackingService
{
    public partial HashSet<string> GetAllFiles(CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        var allfiles = Directory.GetFiles(_options.AssetsPathFull, "*.*", SearchOption.AllDirectories);

        return allfiles.AsParallel().AsOrdered().Where(ValidatePathByPattern).ToHashSet();
    }
}