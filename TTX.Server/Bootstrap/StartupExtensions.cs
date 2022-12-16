using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TTX.Data.Services.Acquisition;
using TTX.Data.Services.Workspace;

namespace TTX.Server.Bootstrap;

public static class StartupExtensions
{
    public static IServiceCollection SetupWorkspace(this IServiceCollection services, Action<WorkspaceOptions>? config = null)
    {
        services
            .AddOptions<WorkspaceOptions>()
            .BindConfiguration(nameof(WorkspaceOptions))
            .Validate(options => !string.IsNullOrEmpty(options.Path),
            $"The configuration key '{nameof(WorkspaceOptions)}:{nameof(WorkspaceOptions.Path)} must not be empty.");

        services.AddSingleton<IWorkspaceService, WorkspaceService>();

        if (config != null)
            services.Configure(config);

        return services;
    }

    public static IServiceCollection SetupDatabase(this WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("TTXAssets")
            ?? throw new Exception("Workspace path is missing from appsettings.");
        return builder.Services;
    }

    public static void AttachDataServices(this IServiceCollection services)
    {
        services.AddSingleton<IAcquisitionService, AcquisitionService>();
    }
}