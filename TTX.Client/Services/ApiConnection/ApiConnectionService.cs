using TTX.Client.Services.ClientSession;

namespace TTX.Client.Services.ApiConnection;

internal partial class ApiConnectionService : IApiConnectionService
{
    private readonly IClientSessionService _clientSession;

    public ApiConnectionService(IClientSessionService sessionClient)
    {
        _clientSession = sessionClient;
    }
}