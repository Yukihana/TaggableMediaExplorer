using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.Messages;

namespace TTX.Data.Shared.BaseClasses;

public abstract class ServiceBase<T> where T : IMessage
{
    private Task? _serviceCycle = null;
    private CancellationTokenSource _cts = new();

    public void TryStart()
    {
        if (_serviceCycle != null &&
            (_serviceCycle.Status == TaskStatus.Running ||
            _serviceCycle.Status == TaskStatus.WaitingToRun))
            return;
        _cts = new CancellationTokenSource();
        _serviceCycle = Task.Run(() => DoCycle(_cts.Token));
    }

    public async Task TryEnd()
    {
        _cts.Cancel();
        if (_serviceCycle != null)
        {
            await _serviceCycle.ConfigureAwait(false);
            _serviceCycle = null;
        }
    }

    public async Task DoCycle(CancellationToken token = default)
    {
        while (!_cts.IsCancellationRequested)
        {
            try
            {
                T? job = await Task.Run(() => GetNext(token), token).ConfigureAwait(false);
                if (job == null)
                    return;
                await Task.Run(() => ProcessNext(job, token), token).ConfigureAwait(false);
            }
            catch { }
        }
    }

    public abstract Task<T?> GetNext(CancellationToken token = default);

    public abstract Task ProcessNext(T job, CancellationToken token = default);
}