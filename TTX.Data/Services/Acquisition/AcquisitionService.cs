using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Services.Communications;
using TTX.Data.Services.Logger;
using TTX.Data.Shared.BaseClasses;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Services.Acquisition;

/// <summary>
/// Class used for acquiring assets on startup.
/// </summary>
public class AcquisitionService : ServiceBase<AcquisitionCommand>, IAcquisitionService
{
    private readonly IMessageBus _bus;
    private readonly ILoggerService _logger;

    private readonly AcquisitionOptions _options;

    public AcquisitionService(IMessageBus bus, ILoggerService logger, AcquisitionOptions options)
    {
        _bus = bus;
        _logger = logger;
        _options = options;
    }

    public override async Task<AcquisitionCommand> GetNext(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public override async Task ProcessNext(AcquisitionCommand message, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task ScanAllFiles()
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
        await _bus.Queue(new AssetQueue() { Paths = finallist.ToArray() }).ConfigureAwait(false);
    }

    public async Task StartWatcher()
    {
    }

    public async Task StopWatcher()
    {
    }

    public bool ValidatePath(string path)
    {
        throw new NotImplementedException();
        // Check blacklist/whitelist here (for individual paths)
    }
}