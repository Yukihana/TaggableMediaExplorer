using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.SessionClient;

internal class SessionClientService
{
    private readonly HttpClient _client = new();
    public string Scheme { get; set; } = "http";
    public string BaseAddress { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 5224;

    public async Task<string?> Get(string path, string? query = null, CancellationToken token = default)
    {
        UriBuilder uri = new()
        {
            Scheme = Scheme,
            Host = BaseAddress,
            Port = Port,
            Path = path
        };
        if (query != null)
            uri.Query = query;

        try
        {
            HttpRequestMessage request = new(HttpMethod.Get, uri.Uri);
            HttpResponseMessage response = await _client.SendAsync(request, token).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            SessionContext.LoggerLogic.LogEntry("Get operation failed", this, uri.Query, ex);
            return null;
        }
    }
}