using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Services.Legacy.AssetsIndexer;
using TTX.Services.Legacy.TagsIndexer;

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
        app.Services.LoadData();
    }

    private static readonly List<Task> _loadTasks = new();

    public static void LoadData(this IServiceProvider provider)
    {
        // Assets
        var assetsIndexer = provider.GetRequiredService<IAssetsIndexerService>();
        Task assetsTask = Task.Run(async () => await assetsIndexer.StartIndexing().ConfigureAwait(false));
        _loadTasks.Add(assetsTask);
        assetsTask.ContinueWith(_loadTasks.Remove);

        // Tags
        var tagsIndexer = provider.GetRequiredService<ITagsIndexerService>();
        Task tagsTask = Task.Run(async () => await tagsIndexer.Reload().ConfigureAwait(false));
        _loadTasks.Add(tagsTask);
        tagsTask.ContinueWith(_loadTasks.Remove);
    }
}