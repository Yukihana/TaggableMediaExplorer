using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Services.Communications;
using TTX.Data.Services.Notification;
using TTX.Data.Shared.BaseClasses;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Services.Acquisition;

/// <summary>
/// Class used for acquiring assets on startup.
/// </summary>
public partial class AcquisitionService : ServiceBase, IAcquisitionService
{
    private readonly INotificationService _notifier;

    private readonly IAcquisitionOptions _options;

    private readonly HashSet<Type> _messageTypes = new() { typeof(AcquisitionCommand) };
    public override HashSet<Type> MessageTypes => _messageTypes;

    public AcquisitionService(IMessageBus bus, INotificationService logger, IAcquisitionOptions options) : base(bus, 1)
    {
        _notifier = logger;
        _options = options;
    }

    protected override async Task ProcessMessage(IMessage message, CancellationToken token = default)
    {
        if (message is AcquisitionCommand command)
        {
            if (command.CommandValue == AcquisitionCommands.ScanAll)
                await ScanAllFiles();
        }
    }

    // JOBS

    public void DoStartup()
    {
        _ = Task.Run(async () =>
        {
            await ScanAllFiles();
            StartWatcher();
        });
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
        await SendMessage(new AssetQueue() { Paths = finallist.ToArray() }).ConfigureAwait(false);
    }
}