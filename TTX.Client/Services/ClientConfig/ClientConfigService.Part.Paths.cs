namespace TTX.Client.Services.ClientConfig;

internal partial class ClientConfigService
{
    public string BaseDirectory
        => _options.BaseDirectory;

    // Previews

    public string PreviewsPath
        => _options.PreviewsPath;

    public string DefaultPreviewsExtension
        => _options.DefaultPreviewsExtension;

    // AssetsCache

    public string AssetsCachePath
        => _options.AssetsCachePath;

    public string CachedAssetsExtension
        => _options.CachedAssetsExtension;
}