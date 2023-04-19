using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.ClientConfig;

namespace TTX.Client.Services.AssetLoader;

internal partial class AssetLoaderService : IAssetLoaderService
{
    private readonly IClientConfigService _clientConfig;
    private readonly IApiConnectionService _apiConnection;
    private readonly ILogger<AssetLoaderService> _logger;
    private readonly string _dirPath;

    public AssetLoaderService(
        IClientConfigService clientConfig,
        IApiConnectionService apiConnection,
        ILogger<AssetLoaderService> logger)
    {
        _clientConfig = clientConfig;
        _apiConnection = apiConnection;
        _logger = logger;

        _dirPath = Path.Combine(_clientConfig.BaseDirectory, _clientConfig.AssetsCachePath);
    }

    public async Task<string> GetCachedPath(string idString, DateTime lastUpdatedUtc, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        string assetPath = GetPathFromId(idString);
        if (!ValidateCache(assetPath, lastUpdatedUtc))
            await DownloadAsset(idString, assetPath, ctoken).ConfigureAwait(false);

        return assetPath;
    }

    private bool ValidateCache(string assetPath, DateTime lastUpdatedUtc)
    {
        Directory.CreateDirectory(_dirPath);
        if (!File.Exists(assetPath))
            return false;
        FileInfo assetInfo = new(assetPath);
        if (assetInfo.CreationTimeUtc < lastUpdatedUtc)
            return false;

        return assetInfo.Length != 0;
    }

    private async Task DownloadAsset(string idString, string assetPath, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        _logger.LogInformation("Downloading asset {id}.", idString);
        byte[] assetContent = await _apiConnection.DownloadAsset(idString, ctoken).ConfigureAwait(false);
        await File.WriteAllBytesAsync(assetPath, assetContent, ctoken).ConfigureAwait(false);
        _logger.LogInformation("Wrote {length} bytes to {path}.", assetContent.Length, assetPath);
    }

    private string GetPathFromId(string idString)
        => Path.Combine(_dirPath, $"{idString}{_clientConfig.CachedAssetsExtension}");
}