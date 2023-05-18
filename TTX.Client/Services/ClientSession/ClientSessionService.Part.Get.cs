using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.ClientSession;

internal partial class ClientSessionService
{
    public async Task<T> GetState<T>(string path, string? query = null, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        var uri = GetUri(path, query);
        using HttpRequestMessage request = new(HttpMethod.Get, uri);

        using HttpResponseMessage response = await _client.SendAsync(request, ctoken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Unsuccessful response from request to {path}. Reason = {reason}",
                uri.PathAndQuery, response.ReasonPhrase);
            throw new HttpRequestException($"Unsuccessful response recieved.");
        }

        return await response.ProcessAsState<T>(ctoken).ConfigureAwait(false);
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