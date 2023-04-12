using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.ClientSession;

internal interface IClientSessionService
{
    Task<string?> Get(string path, string query, CancellationToken token);

    Task<byte[]> DownloadUsingGet(string v1, string v2, CancellationToken ctoken);
}