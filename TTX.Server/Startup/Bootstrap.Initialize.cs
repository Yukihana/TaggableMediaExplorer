using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;
using TTX.Data.ServerData;
using TTX.Services.ControlLayer.AssetIndexing;

namespace TTX.Server.Startup;

public static partial class BootStrap
{
    /// <summary>
    /// Validate the database (Migration Update if applicable) and send initial data to the relevant services.
    /// </summary>
    /// <param name="app"></param>
    public static void InitializeServices(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        // Database setup
        scope.ServiceProvider.GetRequiredService<AssetsContext>().Database.Migrate();

        // Load
        _ = app.Services.LoadData();
    }

    public static async Task LoadData(this IServiceProvider provider)
    {
        try
        {
            // Assets
            var assetsIndexer = provider.GetRequiredService<IAssetIndexingService>();
            await assetsIndexer.StartIndexing().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Encountered an error trying to initialize services.");
        }
    }
}