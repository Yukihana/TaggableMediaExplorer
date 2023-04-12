using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Library.ThreadingHelpers;

namespace TTX.Client.Services.GuiSync;

internal class GuiSyncService : IGuiSyncService
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

    // Context Sync : Action

    public async Task DispatchActionAsync(Action dispatchAction, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        await _options.SyncContext.SendAsync(state => dispatchAction(), null).ConfigureAwait(false);
    }

    public async Task DispatchActionAsync<TIn>(Action<TIn> dispatchAction, TIn inputData, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        await _options.SyncContext.SendAsync((state) =>
        {
            if (state is TIn t)
                dispatchAction(t);
        }, inputData).ConfigureAwait(false);
    }

    // Context Sync : Func

    public async Task<TOut?> DispatchFuncAsync<TOut>(Func<TOut> dispatchAction, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        TOut? result = default;
        await _options.SyncContext.SendAsync(state =>
        {
            var r = dispatchAction();
            result = r;
        }, null).ConfigureAwait(false);

        return result;
    }

    public async Task<TOut?> DispatchFuncAsync<TIn, TOut>(Func<TIn, TOut> dispatchAction, TIn inputData, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        TOut? result = default;
        await _options.SyncContext.SendAsync(state =>
        {
            if (state is TIn t)
                result = dispatchAction(t);
        }, inputData).ConfigureAwait(false);

        return result;
    }

    // Context Sync : Post

    public void DispatchPost(Action dispatchAction, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        _options.SyncContext.Post((state) => dispatchAction(), null);
    }

    public void DispatchPost<TIn>(Action<TIn> dispatchAction, TIn inputData, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();

        _options.SyncContext.Post((state) =>
        {
            if (state is TIn t)
                dispatchAction(t);
        }, inputData);
    }
}