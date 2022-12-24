using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Messages;
using TTX.Services.Communications;

namespace TTX.Services.DbSync;

/// <summary>
/// The service for handling database read/write operations.
/// </summary>
public partial class DbSyncService : ServiceBase, IDbSyncService
{
    private readonly IDbSyncOptions _options;
    private readonly IDbContextFactory<AssetsContext> _contextFactory;
    public override string Identifier => _options.DbSyncSID;

    public DbSyncService(IMessageBus bus, IDbContextFactory<AssetsContext> contextFactory, IDbSyncOptions options) : base(bus, 1)
    {
        _contextFactory = contextFactory;
        _options = options;
    }

    protected override async Task ProcessMessage(IMessage message, CancellationToken token = default)
    {
        if (message is ServiceCommand command)
        {
            if (command.CommandString.Equals(DbSyncCommands.ReadAssetsInfoTable, StringComparison.OrdinalIgnoreCase))
                await ReadAssetsInfoTable(token);
            if (command.CommandString.Equals(DbSyncCommands.ReadTagsInfoTable, StringComparison.OrdinalIgnoreCase))
                await ReadTagsInfoTable(token);
        }
    }
}