using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.ClientConfig;

namespace TTX.Client.Services.PreviewLoader;

internal class PreviewLoaderService : IPreviewLoaderService
{
    private readonly IClientConfigService _clientConfig;
    private readonly IApiConnectionService _apiConnection;
    private readonly ILogger<PreviewLoaderService> _logger;

    private readonly string _dirPath;

    public PreviewLoaderService(
        IClientConfigService clientConfig,
        IApiConnectionService apiConnection,
        ILogger<PreviewLoaderService> logger)
    {
        _clientConfig = clientConfig;
        _apiConnection = apiConnection;
        _logger = logger;

        _dirPath = Path.Combine(_clientConfig.BaseDirectory, _clientConfig.PreviewsPath);
    }

    public async Task<string> GetDefaultPreview(string idString, DateTime lastUpdatedUtc, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        string previewPath = GetPreviewPath(idString);
        if (!ValidatePreview(previewPath, lastUpdatedUtc))
            await DownloadPreview(idString, previewPath, ctoken).ConfigureAwait(false);

        return previewPath;
    }

    private bool ValidatePreview(string previewPath, DateTime lastUpdatedUtc)
    {
        Directory.CreateDirectory(_dirPath);
        if (!File.Exists(previewPath))
            return false;
        FileInfo previewInfo = new(previewPath);
        if (previewInfo.CreationTimeUtc < lastUpdatedUtc)
            return false;
        // Optionally add format checking without relying on System.Drawing.Common
        // Use reference:
        // https://stackoverflow.com/questions/210650/validate-image-from-file-in-c-sharp
        return previewInfo.Length != 0;
    }

    private async Task DownloadPreview(string idString, string previewPath, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        _logger.LogInformation("Downloading default preview of {id}.", idString);
        byte[] previewData = await _apiConnection.DownloadDefaultPreview(idString, ctoken).ConfigureAwait(false);
        await File.WriteAllBytesAsync(previewPath, previewData, ctoken).ConfigureAwait(false);
        _logger.LogInformation("Wrote {length} bytes to {path}.", previewData.Length, previewPath);
    }

    private string GetPreviewPath(string idString)
        => Path.Combine(_dirPath, $"{idString}{_clientConfig.DefaultPreviewsExtension}");
}