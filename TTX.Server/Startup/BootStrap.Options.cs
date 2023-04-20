using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;
using TTX.Services;

namespace TTX.Server.Startup;

public static partial class BootStrap
{
    /// <summary>
    /// Fetch the profile path from the appsettings.json
    /// </summary>
    public static string GetProfilePath(this IConfiguration configuration)
        => configuration.GetValue<string>("Startup:ServerProfile")
        ?? throw new NullReferenceException("Create 'Startup/ServerProfile' in 'appsettings.json'");

    /// <summary>
    /// Create runtime configurations for the server.
    /// </summary>
    public static RuntimeConfig AttachRuntimeConfig(this IServiceCollection services, string profilePath)
    {
        // Config: Server
#if DEBUG
        string serverPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            ?? throw new NullReferenceException("Cannot acquire path to FFMPEG.");
#else
        string serverPath = Environment.CurrentDirectory;
#endif

        // Config: Profile
        string profileRoot = Path.GetDirectoryName(profilePath)
            ?? throw new InvalidDataException($"Unable to infer the profile root path from '{profilePath}'");
        string profileFilename = Path.GetFileName(profilePath);

        // Compile and finish
        var runtimeConfig = new RuntimeConfig()
        {
            ServerPath = serverPath,
            ProfileRoot = profileRoot,
            ProfileFilename = profileFilename,
        };
        services.AddSingleton<IRuntimeConfig>(runtimeConfig);
        return runtimeConfig;
    }

    /// <summary>
    /// Attach current runtime profile parameters.
    /// </summary>
    /// <param name="profilePath">The path to the profile.</param>
    public static WorkspaceProfile AttachWorkspaceProfile(this IServiceCollection services, string profilePath)
    {
        WorkspaceProfile profile = new();

        if (File.Exists(profilePath))
        {
            profile = JsonSerializer.Deserialize<WorkspaceProfile>(
                File.ReadAllText(profilePath), JsonOptions)
                ?? throw new InvalidDataException($"Unable to load data from profile at '{profilePath}'");
        }
        else
        {
            File.WriteAllText(profilePath, JsonSerializer.Serialize(profile, JsonOptions));
            throw new InvalidDataException("Unable to load profile. If this is a first run, then a new profile would have been created. Please run the program again after configuring the values in the profile.");
        }

        services.AddSingleton<IWorkspaceProfile>(profile);
        return profile;
    }
}