using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.ClientConfig;
using TTX.Client.Services.ClientSession;
using TTX.Client.Services.GuiConductor;
using TTX.Client.Services.GuiSync;
using TTX.Client.Services.LoginGui;
using TTX.Client.Services.MainGui;
using TTX.Client.Services.PreviewLoader;
using TTX.Library.ObjectHelpers;

namespace TTX.Client;

public static partial class ClientContextHost
{
    private static IClientOptions? _options = null;
    private static IHost? ServicesContainer = null;

    public static void BuildServices(IClientOptions options)
    {
        _options = options;

        if (ServicesContainer is not null)
            throw new InvalidOperationException("The services container has already been built. Unable to recreate it.");

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton(options);

        // Config services
        builder.Services.AddSingleton<IGuiSyncService, GuiSyncService>();
        builder.Services.AddSingleton<IClientConfigService, ClientConfigService>();
        builder.Services.AddSingleton<IClientSessionService, ClientSessionService>();

        // Core services
        builder.Services.AddSingleton<IApiConnectionService, ApiConnectionService>();

        // Content services
        builder.Services.AddSingleton<IPreviewLoaderService, PreviewLoaderService>();

        // Gui logic
        builder.Services.AddSingleton<ILoginGuiService, LoginGuiService>();
        builder.Services.AddSingleton<IMainGuiService, MainGuiService>();

        // Control
        builder.Services.AddSingleton<IGuiConductorService, GuiConductorService>();

        ServicesContainer = builder.Build();
    }

    public static void Start()
    {
        // Start the DI
        if (ServicesContainer is null)
            throw new NullReferenceException("Services container needs to be built before starting.");
        ServicesContainer.Start();

        // Start gui routine
        IGuiConductorService guiConductor = ServicesContainer.Services
            .GetService<IGuiConductorService>()
            .ThrowOnReferenceNull($"Unable to access a service of type {nameof(IGuiConductorService)}.");
        guiConductor.StartRoutine();
    }

    internal static T GetService<T>()
    {
        if (ServicesContainer is null)
            throw new NullReferenceException("Services container has not been initialized.");
        return ServicesContainer.Services.GetService<T>() ??
            throw new InvalidOperationException($"A service of type {typeof(T)} has not been initialized.");
    }
}