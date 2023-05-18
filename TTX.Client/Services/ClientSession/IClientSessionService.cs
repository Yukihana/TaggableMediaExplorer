using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.ClientSession;

internal interface IClientSessionService
{
    // Get

    Task<string> Get(string path, string? query, CancellationToken token = default);

    Task<byte[]> GetOctet(string path, string? query, CancellationToken ctoken = default);

    // Patch

    Task<string> Patch(string path, string content, string? query, CancellationToken token = default);

    // Post

    Task<string> Post(string path, string content, string? query, CancellationToken ctoken = default);

    Task<TOut> PostStateToState<TIn, TOut>(string path, TIn request, CancellationToken ctoken = default);
}