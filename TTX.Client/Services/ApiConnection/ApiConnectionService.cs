using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ClientSession;
using TTX.Library.Configurations;
using TTX.Library.Helpers;

namespace TTX.Client.Services.ApiConnection;

internal partial class ApiConnectionService : IApiConnectionService
{
    private readonly IClientSessionService _clientSession;

    public ApiConnectionService(IClientSessionService sessionClient)
    {
        _clientSession = sessionClient;
    }

    // Base methods

    private static T ProcessResponse<T>(string? responseString)
    {
        if (string.IsNullOrEmpty(responseString))
            throw new InvalidDataException("Cannot deserialize empty response data.");
        T response = responseString.DeserializeJsonResponse<T>() ??
            throw new NullReferenceException("Deserialization failed.");

        return response;
    }

    private async Task<T> Get<T>(string path, string query, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        string responseString = await _clientSession.Get(path, query, ctoken).ConfigureAwait(false);
        return ProcessResponse<T>(responseString);
    }

    private async Task<TOut> Patch<TIn, TOut>(string path, TIn request, string? query, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        string requestString = JsonSerializer.Serialize(request, JsonHelper.ApiEgressOptions);
        string responseString = await _clientSession.Patch(path, requestString, query, ctoken).ConfigureAwait(false);
        return ProcessResponse<TOut>(responseString);
    }

    private async Task<TOut> Post<TIn, TOut>(string path, TIn request, string? query, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        string requestString = JsonSerializer.Serialize(request, JsonHelper.ApiEgressOptions);
        string responseString = await _clientSession.Post(path, requestString, query, ctoken).ConfigureAwait(false);
        return ProcessResponse<TOut>(responseString);
    }
}