using System;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.GuiSync;

internal interface IGuiSyncService
{
    CancellationToken CancellationToken { get; }

    void CancelActiveTasks();

    void ThrowIfNotOnMainThread();

    // Dispatch Action

    Task DispatchActionAsync(Action dispatchAction, CancellationToken ctoken = default);

    Task DispatchActionAsync<TIn>(Action<TIn> dispatchAction, TIn inputData, CancellationToken ctoken = default);

    // Dispatch Func

    Task<TOut?> DispatchFuncAsync<TOut>(Func<TOut> dispatchAction, CancellationToken ctoken = default);

    Task<TOut?> DispatchFuncAsync<TIn, TOut>(Func<TIn, TOut?> dispatchAction, TIn inputData, CancellationToken ctoken = default);

    Task<TOut?> DispatchFuncAsyncN<TIn, TOut>(Func<TIn?, TOut?> dispatchAction, TIn inputData, CancellationToken ctoken = default);

    // Dispatch Post

    void DispatchPost(Action dispatchAction, CancellationToken ctoken = default);

    void DispatchPost<TIn>(Action<TIn> dispatchAction, TIn inputData, CancellationToken ctoken = default);
}