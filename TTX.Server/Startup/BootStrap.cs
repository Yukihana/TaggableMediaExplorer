using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;
using TTX.Data.Services.Acquisition;

namespace TTX.Server.Startup;

public static class BootStrap
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

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
            profile.ServerRoot = profilePath;
            return profile;
        }

        File.WriteAllText(profilePath, JsonSerializer.Serialize(
                new WorkspaceProfile(), JsonOptions));
        return null;
    }

    public static IServiceCollection SetupDatabase(this WebApplicationBuilder builder, string serverRoot)
    {
        string connectionString = builder.Configuration.GetConnectionString("TTXAssets")
            ?? throw new Exception("Workspace path is missing from appsettings.");
        return builder.Services;
    }

    public static void AttachDataServices(this IServiceCollection services, WorkspaceProfile profile)
    {
        services.AddSingleton<IAcquisitionService>();
    }
}