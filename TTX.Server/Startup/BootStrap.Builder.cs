using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Library.Helpers;
using TTX.Services;
using TTX.Services.Acquisition;
using TTX.Services.AssetsIndexer;
using TTX.Services.Communications;
using TTX.Services.DbSync;
using TTX.Services.Indexer;
using TTX.Services.Metadata;
using TTX.Services.Notification;
using TTX.Services.TagsIndexer;
using TTX.Services.Watcher;

namespace TTX.Server.Startup;

public static partial class BootStrap
{
    /// <summary>
    /// Loads or creates a workspace profile at the specified path. Returns null on fail.
    /// </summary>
    /// <param name="profilePath">The path to the profile.</param>
    public static WorkspaceProfile? LoadWorkspaceProfile(this IConfiguration configuration)
    {
        string profilePath = configuration.GetValue<string>("Startup:ServerProfile")
            ?? throw new NullReferenceException("Create 'Startup/ServerProfile' in 'appsettings.json'");

        if (File.Exists(profilePath))
        {
            WorkspaceProfile profile = JsonSerializer.Deserialize<WorkspaceProfile>(
                File.ReadAllText(profilePath), JsonOptions)
                ?? throw new InvalidDataException($"Unable to load data from profile at '{profilePath}'");
            profile.ServerRoot = Path.GetDirectoryName(profilePath)
                ?? throw new InvalidDataException($"Unable to infer a server root directory path from '{profilePath}'");
            return profile;
        }

        File.WriteAllText(profilePath, JsonSerializer.Serialize(
                new WorkspaceProfile(), JsonOptions));
        return null;
    }

    /// <summary>
    /// Setup database connections.
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// <param name="serverRoot">Current Path</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection AttachDatabase(this WebApplicationBuilder builder, WorkspaceProfile profile)
    {
        string connectionString = builder.Configuration.GetConnectionString("AssetsDbString")
            ?? throw new Exception("Workspace path is missing from appsettings.");
        string pathToDbFile = Path.IsPathFullyQualified(profile.AssetsDbFilename)
            ? profile.AssetsDbFilename
            : Path.Combine(profile.ServerRoot, profile.AssetsDbFilename);
        connectionString = connectionString.Replace("@", pathToDbFile);

        builder.Services.AddDbContext<AssetsContext>(
            options => options.UseSqlite(connectionString.Replace("@", pathToDbFile)));
        return builder.Services;
    }

    /// <summary>
    /// Attach options required by various services.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="profile"></param>
    public static void AttachOptions(this IServiceCollection services, WorkspaceProfile profile)
    {
        services.AddSingleton<IAssetsIndexerOptions>(profile.ExtractOptions<AssetsIndexerOptions>());
        services.AddSingleton<ITagsIndexerOptions>(profile.ExtractOptions<TagsIndexerOptions>());

        services.AddSingleton<IDbSyncOptions>(profile.ExtractOptions<DbSyncOptions>());
        services.AddSingleton<IWatcherOptions>(profile.ExtractOptions<WatcherOptions>());

        // Legacy
        services.AddSingleton<IAcquisitionOptions>(profile.ExtractOptions<AcquisitionOptions>());
        services.AddSingleton<IMetadataOptions>(profile.ExtractOptions<MetadataOptions>());
        services.AddSingleton<IDbSyncOptions>(profile.ExtractOptions<DbSyncOptions>());
    }

    /// <summary>
    /// Attach the services.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="profile"></param>
    public static void AttachDataServices(this IServiceCollection services)
    {
        services.AddSingleton<IAssetsIndexerService, AssetsIndexerService>();
        services.AddSingleton<ITagsIndexerService, TagsIndexerService>();

        services.AddSingleton<IDbSyncService, DbSyncService>();
        services.AddSingleton<IWatcherService, WatcherService>();

        // Legacy
        services.AddSingleton<IMessageBus, MessageBus>();

        // ServiceBase services
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IAcquisitionService, AcquisitionService>();
        services.AddSingleton<IMetadataService, MetadataService>();
        services.AddSingleton<IDbSyncService, DbSyncService>();
    }

    // Helper methods
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static TOptions ExtractOptions<TOptions>(this WorkspaceProfile profile) where TOptions : IServiceOptions, new()
    {
        var options = profile.CopyValues<TOptions>().CopyFullyDecoupled();
        options.Initialize();
        return options;
    }
}