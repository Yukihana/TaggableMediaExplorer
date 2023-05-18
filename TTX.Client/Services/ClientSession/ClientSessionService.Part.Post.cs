using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.ClientSession;

internal partial class ClientSessionService
{
    public async Task<TOut> PostStateToState<TIn, TOut>(string path, TIn requestContent, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        var uri = GetUri(path);
        using HttpResponseMessage response = await _client
            .PostAsJsonAsync(uri, requestContent, ctoken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Unsuccessful response from request to {path}. Data = {request}. Reason = {reason}",
                uri.PathAndQuery, requestContent, response.ReasonPhrase);
            throw new HttpRequestException($"Unsuccessful response recieved.");
        }

        return await response.ProcessAsState<TOut>(ctoken).ConfigureAwait(false);
    }
}