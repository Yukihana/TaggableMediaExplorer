using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnection;

namespace TTX.Client.Services.ClientSession;

internal partial class ClientSessionService
{
    public async Task<string> Patch(string path, string content, string? query = null, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        var uri = GetUri(path, query);
        using HttpContent requestContent = content.ToStringContent();

        _logger.LogInformation("Sending patch request to {path}. Data = {request}", uri.PathAndQuery, content);
        using HttpResponseMessage response = await _client.PatchAsync(uri, requestContent, ctoken).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync(ctoken).ConfigureAwait(false);
    }
}