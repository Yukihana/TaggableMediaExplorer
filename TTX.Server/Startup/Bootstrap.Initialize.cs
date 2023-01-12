using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Services.AssetsIndexer;
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
        scope.LoadData();
    }

    private static readonly List<Task> _loadTasks = new();

    public static void LoadData(this IServiceScope scope)
    {
        // Assets
        var assetsIndexer = scope.ServiceProvider.GetRequiredService<IAssetsIndexerService>();
        Task assetsTask = Task.Run(assetsIndexer.Reload);
        _loadTasks.Add(assetsTask);
        assetsTask.ContinueWith(task => _loadTasks.Remove(assetsTask));

        // Tags
        var tagsIndexer = scope.ServiceProvider.GetRequiredService<ITagsIndexerService>();
        Task tagsTask = Task.Run(tagsIndexer.Reload);
        _loadTasks.Add(tagsTask);
        tagsTask.ContinueWith(task => _loadTasks.Remove(tagsTask));
    }
}