using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTX.Data.ServerData;
using TTX.Services.ControlLayer.AssetIndexing;
using TTX.Services.Legacy.TagsIndexer;

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
        app.Services.LoadData();
    }

    private static readonly List<Task> _loadTasks = new();

    public static void LoadData(this IServiceProvider provider)
    {
        // Assets
        var assetsIndexer = provider.GetRequiredService<IAssetIndexingService>();
        Task assetsTask = Task.Run(async () => await assetsIndexer.StartIndexing().ConfigureAwait(false));
        _loadTasks.Add(assetsTask);
        _ = assetsTask.ContinueWith(_loadTasks.Remove, TaskScheduler.Default);

        // Tags
        var tagsIndexer = provider.GetRequiredService<ITagsIndexerService>();
        Task tagsTask = Task.Run(async () => await tagsIndexer.Reload().ConfigureAwait(false));
        _loadTasks.Add(tagsTask);
        _ = tagsTask.ContinueWith(_loadTasks.Remove, TaskScheduler.Current);
    }
}