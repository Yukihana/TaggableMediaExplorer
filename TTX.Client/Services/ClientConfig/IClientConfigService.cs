using TTX.Client.Services.LoginGui;
using TTX.Client.Services.MainGui;
using TTX.Client.Services.TagSelectorGui;

namespace TTX.Client.Services.ClientConfig;

// Service-side api for the client options
internal interface IClientConfigService
{
    // Runtime

    // Gui

    ILoginView CreateLoginView();

    IMainView CreateMainView();

    ITagSelectorView CreateTagSelectorView();

    void Shutdown();

    // Paths

    string BaseDirectory { get; }
    string PreviewsPath { get; }
    string DefaultPreviewsExtension { get; }
    string AssetsCachePath { get; }
    string CachedAssetsExtension { get; }
}