using TTX.Client.Services.LoginGui;
using TTX.Client.Services.MainGui;

namespace TTX.Client.Services.ClientConfig;

// Service-side api for the client options
internal interface IClientConfigService
{
    // Runtime

    // Gui

    ILoginView CreateLoginView();

    IMainView CreateMainView();

    void Shutdown();

    // Paths

    string BaseDirectory { get; }

    string PreviewsPath { get; }
}