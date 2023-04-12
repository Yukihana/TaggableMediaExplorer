using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.ClientSession;

internal class ClientSessionService : IClientSessionService
{
    private readonly ILogger<ClientSessionService> _logger;
    private readonly IClientOptions _options;

    private readonly HttpClient _client = new();

    public ClientSessionService(ILogger<ClientSessionService> logger, IClientOptions options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task<string?> Get(string path, string? query = null, CancellationToken token = default)
    {
        // Build request
        UriBuilder uri = new()
        {
            Scheme = _options.Scheme,
            Host = _options.BaseAddress,
            Port = _options.Port,
            Path = path
        };
        if (query != null)
            uri.Query = query;

        // Attempt get request
        try
        {
            using HttpRequestMessage request = new(HttpMethod.Get, uri.Uri);
            using HttpResponseMessage response = await _client.SendAsync(request, token).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get operation failed for {query}", uri.Query);
            return null;
        }
    }

    public async Task<byte[]> DownloadUsingGet(string path, string? query = null, CancellationToken token = default)
    {
        // Build request
        UriBuilder uri = new()
        {
            Scheme = _options.Scheme,
            Host = _options.BaseAddress,
            Port = _options.Port,
            Path = path
        };
        if (query != null)
            uri.Query = query;

        // Download as octet stream
        using MemoryStream ms = new();
        using HttpRequestMessage request = new(HttpMethod.Get, uri.Uri);
        using HttpResponseMessage response = await _client.SendAsync(request, token).ConfigureAwait(false);
        using Stream responseStream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
        responseStream.CopyTo(ms);
        return ms.ToArray();
    }
}