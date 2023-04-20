using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;
using TTX.Data;
using TTX.Services.ApiLayer.AssetCardData;
using TTX.Services.ApiLayer.AssetContent;
using TTX.Services.ApiLayer.AssetSearch;
using TTX.Services.ApiLayer.AssetSnapshotData;
using TTX.Services.ControlLayer.AssetIndexing;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.Legacy.TagsIndexer;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.ProcessingLayer.AssetSynchronisation;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;
using TTX.Services.StorageLayer.AssetPreview;
using TTX.Services.StorageLayer.MediaCodec;
using TTX.Services.TagsIndexer;

namespace TTX.Server.Startup;

public static partial class BootStrap
{
    /// <summary>
    /// Setup database connections.
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// <param name="profile">Current Path</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection AttachAssetsDatabase(
        this WebApplicationBuilder builder,
        WorkspaceProfile profile,
        RuntimeConfig config)
    {
        // Get the connection pattern ("Data Source=@")
        string connectionString = builder.Configuration.GetConnectionString("AssetsDbString")
            ?? throw new Exception("Workspace path is missing from appsettings.");

        // Prepare the path to the Db file
        string pathToDbFile
            = Path.IsPathFullyQualified(profile.AssetsDbFilename)
            ? profile.AssetsDbFilename
            : Path.Combine(config.ProfileRoot, profile.AssetsDbFilename);

        // Finalize the connection string
        connectionString = connectionString.Replace("@", pathToDbFile);

        builder.Services.AddDbContext<AssetsContext>(
            options => options.UseSqlite(connectionString),
            optionsLifetime: ServiceLifetime.Singleton);
        builder.Services.AddDbContextFactory<AssetsContext>(
            options => options.UseSqlite(connectionString));

        return builder.Services;
    }

    /// <summary>
    /// Attach the services.
    /// </summary>
    public static void AttachDataServices(this IServiceCollection services)
    {
        // Independents
        services.AddSingleton<IAssetDatabaseService, AssetDatabaseService>();
        services.AddSingleton<IAssetPresenceService, AssetPresenceService>();
        services.AddSingleton<IAssetPreviewService, AssetPreviewService>();
        services.AddSingleton<IAssetTrackingService, AssetTrackingService>();
        services.AddSingleton<IAssetAnalysisService, AssetAnalysisService>();
        services.AddSingleton<IMediaCodecService, MediaCodecService>();

        // Intermediates
        services.AddSingleton<IAssetSynchronisationService, AssetSynchronisationService>();

        // Top layer: Api services
        services.AddSingleton<IAssetSearchService, AssetSearchService>();
        services.AddSingleton<IAssetCardDataService, AssetCardDataService>();
        services.AddSingleton<IAssetSnapshotDataService, AssetSnapshotDataService>();
        services.AddSingleton<IAssetContentService, AssetContentService>();

        // Top layer: Control services
        services.AddSingleton<IAssetIndexingService, AssetIndexingService>();
        services.AddSingleton<ITagsIndexerService, TagsIndexerService>();
    }

    // Helper methods
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };
}