using System;
using System.Threading;
using TTX.Client.Services.LoginGui;
using TTX.Client.Services.MainGui;
using TTX.Client.Services.TagSelectorGui;

namespace TTX.Client;

public interface IClientOptions
{
    // Runtime

    SynchronizationContext SyncContext { get; init; }
    Action ShutdownAction { get; init; }

    // Gui

    Func<ILoginView> LoginViewFactoryMethod { get; init; }
    Func<IMainView> MainViewFactoryMethod { get; init; }
    Func<ITagSelectorView> TagSelectorViewFactoryMethod { get; init; }

    // Paths

    string BaseDirectory { get; init; }
    string PreviewsPath { get; init; }
    string DefaultPreviewsExtension { get; init; }
    string AssetsCachePath { get; init; }
    string CachedAssetsExtension { get; init; }

    // Connection parameters

    public string Scheme { get; init; }
    public string BaseAddress { get; init; }
    public int Port { get; init; }
}