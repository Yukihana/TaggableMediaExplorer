using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;

namespace TTX.Services.Acquisition;

public partial class AcquisitionService
{
    public async Task ScanAllFiles(CancellationToken token = default)
    {
        HashSet<string> allfiles = _options.AssetsPathFull.GetLocalFilePathsByPatterns(new string[] { "*.*" });
        HashSet<string> whitelist = _options.AssetsPathFull.GetLocalFilePathsByPatterns(_options.Whitelist);
        HashSet<string> blacklist = _options.AssetsPathFull.GetLocalFilePathsByPatterns(_options.Blacklist);
        HashSet<string> finaladds = _options.AssetsPathFull.GetLocalFilePathsByPatterns(_options.FinalAdds);

        HashSet<string> finallist = new();

        foreach (string file in allfiles)
        {
            if (finaladds.Contains(file))
            {
                finallist.Add(file);
            }
            else if (blacklist.Contains(file))
            {
                continue;
            }
            else if (whitelist.Contains(file))
            {
                finallist.Add(file);
            }
        }

        var message = new AssetQueue()
        {
            TargetSID = _options.MetadataSID,
            Paths = finallist.ToArray()
        };

        await SendMessage(message, token).ConfigureAwait(false);
    }
}