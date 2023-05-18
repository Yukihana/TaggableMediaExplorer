using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TTX.Library.Helpers;

namespace TTX.Client.Services.ClientSession;

internal static class ClientSessionHelper
{
    public static async Task<T> ProcessAsState<T>(this HttpResponseMessage response, CancellationToken ctoken = default)
    {
        // Extract string
        ctoken.ThrowIfCancellationRequested();

        string responseString = await response.Content.ReadAsStringAsync(ctoken).ConfigureAwait(false);

        // Validate
        ctoken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(responseString))
            throw new InvalidDataException("Cannot deserialize empty response data.");
        T state = responseString.DeserializeJsonResponse<T>() ??
            throw new NullReferenceException("Deserialization failed.");

        return state;
    }
}