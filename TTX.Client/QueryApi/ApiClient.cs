using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.QueryApi;

internal class ApiClient
{
    private readonly HttpClient _client = new();
    public string BaseAddress { get; set; } = "127.0.0.1";

    public async Task<string> Get(string path, string? query = null, CancellationToken token = default)
    {
        UriBuilder uri = new()
        {
            Host = BaseAddress,
            Path = path
        };
        if (query != null)
            uri.Query = query;

        HttpRequestMessage request = new(HttpMethod.Get, uri.Uri);
        HttpResponseMessage response = await _client.SendAsync(request, token).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
    }
}