using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Services.Indexer;
using TTX.Services.TagsIndexer;

namespace TTX.Server.Startup;

public static partial class BootStrap
{
    /// <summary>
    /// Validate the database (Migration Update if applicable) and send initial data to the relevant services.
    /// </summary>
    /// <param name="app"></param>
    public static void InitializeServices(this WebApplication app, WorkspaceProfile profile)
    {
        using var scope = app.Services.CreateScope();

        // Database setup
        scope.ServiceProvider.GetRequiredService<AssetsContext>().Database.Migrate();

        // Load
        scope.LoadAssets();
        scope.LoadTags();
    }

    public static void LoadAssets(this IServiceScope scope)
    {
        var indexer = scope.ServiceProvider.GetRequiredService<IAssetsIndexerService>();
        _ = Task.Run(indexer.Reload);
    }

    public static void LoadTags(this IServiceScope scope)
    {
        var indexer = scope.ServiceProvider.GetRequiredService<ITagsIndexerService>();
        _ = Task.Run(indexer.Reload);
    }
}