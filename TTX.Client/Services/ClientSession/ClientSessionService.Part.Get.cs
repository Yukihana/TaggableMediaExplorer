using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.ClientSession;

internal partial class ClientSessionService
{
    public async Task<string> Get(string path, string? query = null, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        var uri = GetUri(path, query);
        using HttpRequestMessage request = new(HttpMethod.Get, uri);

        _logger.LogInformation("Sending request to {path}.", uri.PathAndQuery);
        using HttpResponseMessage response = await _client.SendAsync(request, ctoken).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync(ctoken).ConfigureAwait(false);
    }

    public async Task<byte[]> GetOctet(string path, string? query = null, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        var uri = GetUri(path, query);
        using HttpRequestMessage request = new(HttpMethod.Get, uri);

        _logger.LogInformation("Sending request to {path}.", uri.PathAndQuery);
        using HttpResponseMessage response = await _client.SendAsync(request, ctoken).ConfigureAwait(false);
        using Stream responseStream = await response.Content.ReadAsStreamAsync(ctoken).ConfigureAwait(false);

        using MemoryStream ms = new();
        responseStream.CopyTo(ms);
        return ms.ToArray();
    }
}