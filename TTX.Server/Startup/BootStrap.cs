using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;
using TTX.Data.Services.Acquisition;
using TTX.Data.Services.Communications;
using TTX.Data.Services.Notification;
using TTX.Data.Shared.BaseClasses;
using TTX.Data.Shared.Helpers;
using TTX.Server.Database;

namespace TTX.Server.Startup;

public static class BootStrap
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
                ?? throw new InvalidDataException($"Unable to infer a server root director path from '{profilePath}'");
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
    public static void AttachOptions(this WebApplicationBuilder builder, WorkspaceProfile profile)
    {
        builder.Services.AddSingleton<IAcquisitionOptions>(profile.ExtractOptions<AcquisitionOptions>());
    }

    /// <summary>
    /// Attach the services.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="profile"></param>
    public static void AttachDataServices(this IServiceCollection services, WorkspaceProfile profile)
    {
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IMessageBus, MessageBus>();
        services.AddSingleton<IAcquisitionService, AcquisitionService>();
    }

    /// <summary>
    /// Validate the database (Migration Update if applicable) and send initial data to the relevant services.
    /// </summary>
    /// <param name="app"></param>
    public static void InitializeServices(this WebApplication app)
    {
        // Validate Database
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AssetsContext>().Database.Migrate();

        // Load database into memory (TODO: Indexing Service)

        // Scan files (TODO: followed by starting filesystem watcher)
        app.Services.GetRequiredService<IAcquisitionService>().DoStartup();
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