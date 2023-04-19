using System;
using System.Threading;
using TTX.Client;
using TTX.Client.Services.LoginGui;
using TTX.Client.Services.MainGui;

namespace TTX.GuiWpf;

public class ClientOptions : IClientOptions
{
    public ClientOptions(SynchronizationContext syncContext)
    {
        SyncContext = syncContext;
    }

    // Runtime

    public SynchronizationContext SyncContext { get; init; }

    public Action ShutdownAction { get; init; }
        = () => { };

    // Gui

    public Func<ILoginView> LoginViewFactoryMethod { get; init; }
        = () => throw new NotImplementedException();

    public Func<IMainView> MainViewFactoryMethod { get; init; }
        = () => throw new NotImplementedException();

    // Paths

    public string BaseDirectory { get; init; } = string.Empty;
    public string PreviewsPath { get; init; } = "Previews";
    public string DefaultPreviewsExtension { get; init; } = ".png"; // Change to cdp? (cached default preview)
    public string AssetsCachePath { get; init; } = "AssetsCache";
    public string CachedAssetsExtension { get; init; } = ".mp4";

    // Connection parameters

    public string Scheme { get; init; } = "http";
    public string BaseAddress { get; init; } = "127.0.0.1";
    public int Port { get; init; } = 5224;
}