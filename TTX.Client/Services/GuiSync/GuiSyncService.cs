using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace TTX.Client.Services.GuiSync;

internal partial class GuiSyncService : IGuiSyncService
{
    private readonly IClientOptions _options;
    private readonly ILogger<GuiSyncService> _logger;

    public GuiSyncService(
        ILogger<GuiSyncService> logger,
        IClientOptions options)
    {
        _logger = logger;
        _options = options;

        _tokenSource = new();
    }

    // Cancellation Token

    private CancellationTokenSource _tokenSource;

    public CancellationToken CancellationToken => _tokenSource.Token;

    public void CancelActiveTasks()
        => _tokenSource.Cancel();

    public void ThrowIfNotOnMainThread()
    {
        if (SynchronizationContext.Current != _options.SyncContext)
            throw new InvalidOperationException("This operation must be run on the main thread.");
    }
}