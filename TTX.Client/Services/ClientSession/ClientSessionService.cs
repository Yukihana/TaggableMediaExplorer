using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using TTX.Library.Helpers;

namespace TTX.Client.Services.ClientSession;

internal partial class ClientSessionService : IClientSessionService
{
    private readonly ILogger<ClientSessionService> _logger;
    private readonly IClientOptions _options;

    private readonly HttpClient _client = new();

    public ClientSessionService(ILogger<ClientSessionService> logger, IClientOptions options)
    {
        _logger = logger;
        _options = options;
    }

    private Uri GetUri(string path, string? query = null)
    {
        UriBuilder uri = new()
        {
            Scheme = _options.Scheme,
            Host = _options.BaseAddress,
            Port = _options.Port,
            Path = path
        };

        if (query != null)
            uri.Query = query;

        return uri.Uri;
    }
}