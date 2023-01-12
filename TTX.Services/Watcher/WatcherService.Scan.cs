using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.Watcher;

public partial class WatcherService
{
    public async Task<HashSet<string>> GetAllFiles(CancellationToken token = default)
    {
        // Fetch
        var tAllfiles = Task.Run(() => _options.AssetsPathFull.GetLocalFilePathsByPatterns(new string[] { "*.*" }));
        var tWhitelist = Task.Run(() => _options.AssetsPathFull.GetLocalFilePathsByPatterns(_options.Whitelist));
        var tBlacklist = Task.Run(() => _options.AssetsPathFull.GetLocalFilePathsByPatterns(_options.Blacklist));
        var tFinaladds = Task.Run(() => _options.AssetsPathFull.GetLocalFilePathsByPatterns(_options.FinalAdds));

        await Task.WhenAll(tAllfiles, tWhitelist, tBlacklist, tFinaladds).ConfigureAwait(false);

        HashSet<string> allfiles = tAllfiles.GetAwaiter().GetResult();
        HashSet<string> whitelist = tWhitelist.GetAwaiter().GetResult();
        HashSet<string> blacklist = tBlacklist.GetAwaiter().GetResult();
        HashSet<string> finaladds = tFinaladds.GetAwaiter().GetResult();

        // Sort

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

        return finallist;
    }
}