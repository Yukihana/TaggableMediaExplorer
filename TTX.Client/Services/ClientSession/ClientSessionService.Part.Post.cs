using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;

namespace TTX.Client.Services.ClientSession;

internal partial class ClientSessionService
{
    public async Task<string> Post(string path, string content, string? query = null, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        var uri = GetUri(path, query);
        using HttpContent requestContent = content.ToStringContent();

        _logger.LogInformation("Sending post request to {path}. Data = {request}", uri.PathAndQuery, content);
        using HttpResponseMessage response = await _client.PostAsync(uri, requestContent, ctoken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Unsuccessful response from request to {path}. Data = {request}. Reason = {reason}",
                uri.PathAndQuery, content, response.ReasonPhrase);
            throw new HttpRequestException($"Unsuccessful response recieved.");
        }

        return await response.Content.ReadAsStringAsync(ctoken).ConfigureAwait(false);
    }

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

        string responseString = await response.Content.ReadAsStringAsync(ctoken).ConfigureAwait(false);
        return ProcessResponse<TOut>(responseString);
    }
}