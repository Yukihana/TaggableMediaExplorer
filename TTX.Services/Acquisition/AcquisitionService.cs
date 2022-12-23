using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;
using TTX.Services.Communications;
using TTX.Services.Notification;

namespace TTX.Services.Acquisition;

/// <summary>
/// Class used for acquiring assets on startup.
/// </summary>
public partial class AcquisitionService : ServiceBase, IAcquisitionService
{
    public override string Identifier => _options.AcquisitionSID;

    private readonly INotificationService _notifier;

    private readonly IAcquisitionOptions _options;

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
            command.TargetSID.Equals(_options.AcquisitionSID, StringComparison.OrdinalIgnoreCase))
        {
            if (command.CommandString.Equals(AcquisitionCommands.ScanAll, StringComparison.OrdinalIgnoreCase))
                await ScanAllFiles(token);
            else if (command.CommandString.Equals(AcquisitionCommands.StartWatcher, StringComparison.OrdinalIgnoreCase))
                StartWatcher();
            else if (command.CommandString.Equals(AcquisitionCommands.StopWatcher, StringComparison.OrdinalIgnoreCase))
                StopWatcher();
        }
    }
}