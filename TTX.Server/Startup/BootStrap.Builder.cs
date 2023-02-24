﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;
using TTX.Data;
using TTX.Services;
using TTX.Services.AbstractionLayer.AssetQuery;
using TTX.Services.ApiLayer.AssetCardData;
using TTX.Services.ApiLayer.AssetSearch;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.Legacy.AssetsIndexer;
using TTX.Services.Legacy.Auxiliary;
using TTX.Services.Legacy.DbSync;
using TTX.Services.Legacy.TagsIndexer;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.ProcessingLayer.AssetSynchronisation;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;
using TTX.Services.TagsIndexer;

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
    /// <param name="profile">Current Path</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection AttachDatabase(this WebApplicationBuilder builder, WorkspaceProfile profile)
    {
        // Get the connection pattern ("Data Source=@")
        string connectionString = builder.Configuration.GetConnectionString("AssetsDbString")
            ?? throw new Exception("Workspace path is missing from appsettings.");

        // Prepare the path to the Db file
        string pathToDbFile = Path.IsPathFullyQualified(profile.AssetsDbFilename)
            ? profile.AssetsDbFilename
            : Path.Combine(profile.ServerRoot, profile.AssetsDbFilename);

        // Finalize the connection string
        connectionString = connectionString.Replace("@", pathToDbFile);

        builder.Services.AddDbContext<AssetsContext>(options => options.UseSqlite(connectionString), optionsLifetime: ServiceLifetime.Singleton);
        builder.Services.AddDbContextFactory<AssetsContext>(options => options.UseSqlite(connectionString));

        return builder.Services;
    }

    /// <summary>
    /// Attach options required by various services.
    /// </summary>
    public static void AttachOptions(this IServiceCollection services, WorkspaceProfile profile)
    {
        services.AddSingleton<IOptionsSet>(profile);
    }

    /// <summary>
    /// Attach the services.
    /// </summary>
    public static void AttachDataServices(this IServiceCollection services)
    {
        // Storage Layer
        services.AddSingleton<IAssetDatabaseService, AssetDatabaseService>();
        services.AddSingleton<IAssetPresenceService, AssetPresenceService>();

        // IncomingLayer
        services.AddSingleton<IAssetTrackingService, AssetTrackingService>();

        // ProcessingLayer
        services.AddSingleton<IAssetSynchronisationService, AssetSynchronisationService>();
        services.AddSingleton<IAssetAnalysisService, AssetAnalysisService>();

        // Unimplemented / Legacy
        services.AddSingleton<IDbSyncService, DbSyncService>();
        services.AddSingleton<IAuxiliaryService, AuxiliaryService>();
        services.AddSingleton<IAssetsIndexerService, AssetsIndexerService>();
        services.AddSingleton<ITagsIndexerService, TagsIndexerService>();

        // Abstraction Layer
        services.AddSingleton<IAssetQueryService, AssetQueryService>();

        // API Layer
        services.AddSingleton<IAssetSearchService, AssetSearchService>();
        services.AddSingleton<IAssetCardDataService, AssetCardDataService>();
    }

    // Helper methods
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };
}