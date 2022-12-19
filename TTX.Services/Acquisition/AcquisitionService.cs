using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Services.Communications;
using TTX.Services.Notification;
using TTX.Data.Messages;

namespace TTX.Services.Acquisition;

/// <summary>
/// Class used for acquiring assets on startup.
/// </summary>
public partial class AcquisitionService : ServiceBase, IAcquisitionService
{
    public override string Identifier => _options.AcquisitionSID;

    private readonly INotificationService _notifier;

    private readonly IAcquisitionOptions _options;

    private readonly HashSet<Type> _messageTypes = new();
    public override HashSet<Type> MessageTypes => _messageTypes;

    public AcquisitionService(IMessageBus bus, INotificationService logger, IAcquisitionOptions options) : base(bus, 1)
    {
        _notifier = logger;
        _options = options;
    }

    public void DoStartup()
    {
        _ = Task.Run(async () =>
        {
            await ScanAllFiles();
            StartWatcher();
        });
    }

    // Process messages

    protected override async Task ProcessMessage(IMessage message, CancellationToken token = default)
    {
        if (message is ServiceCommand command &&
            command.TargetService.Equals(_options.AcquisitionSID, StringComparison.OrdinalIgnoreCase))
        {
            if (command.CommandString.Equals(AcquisitionCommands.ScanAll, StringComparison.OrdinalIgnoreCase))
                await ScanAllFiles();
            else if (command.CommandString.Equals(AcquisitionCommands.StartWatcher, StringComparison.OrdinalIgnoreCase))
                StartWatcher();
            else if (command.CommandString.Equals(AcquisitionCommands.StopWatcher, StringComparison.OrdinalIgnoreCase))
                StopWatcher();
        }
    }
}