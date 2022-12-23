using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Services;
using TTX.Services.Acquisition;
using TTX.Services.Communications;
using TTX.Services.DbSync;
using TTX.Services.Metadata;

namespace TTX.Server.Startup;

public static partial class BootStrap
{
    /// <summary>
    /// Validate the database (Migration Update if applicable) and send initial data to the relevant services.
    /// </summary>
    /// <param name="app"></param>
    public static void InitializeServices(this WebApplication app, WorkspaceProfile profile)
    {
        // Validate Database
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AssetsContext>().Database.Migrate();

        IMessageBus messageBus = app.Services.GetRequiredService<IMessageBus>();

        // Register ServiceBase services with the MessageBus
        messageBus.RegisterServiceBaseServices(app.Services);

        // Load database into memory
        messageBus.StartLoadingDatabase(profile.DbSyncSID);

        // Scan files
        messageBus.StartSyncingAssets(profile.AcquisitionSID);
    }

    public static void RegisterServiceBaseServices(this IMessageBus messageBus, IServiceProvider services)
    {
        messageBus.RegisterService((ServiceBase)services.GetRequiredService<IAcquisitionService>());
        messageBus.RegisterService((ServiceBase)services.GetRequiredService<IMetadataService>());
        messageBus.RegisterService((ServiceBase)services.GetRequiredService<IDbSyncService>());

        Trace.WriteLine($"Registered {messageBus.ServicesCount()} services with the message bus.");
    }

    public static void StartLoadingDatabase(this IMessageBus messageBus, string targetSID)
    {
        _ = Task.Run(async () =>
        {
            await messageBus.SendCommand(targetSID, DbSyncCommands.ReadAssetsInfoTable);
            await messageBus.SendCommand(targetSID, DbSyncCommands.ReadTagsInfoTable);
        });
    }

    public static void StartSyncingAssets(this IMessageBus messageBus, string targetSID)
    {
        _ = Task.Run(async () =>
        {
            await messageBus.SendCommand(targetSID, AcquisitionCommands.ScanAll);
        });
    }
}