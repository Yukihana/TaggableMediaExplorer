using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly HashSet<AssetInfo> _liveSet = new();

    private readonly HashSet<AssetInfo> _stashed = new();

    // Readiness

    private readonly SemaphoreSlim _semaphore = new(1);
    private bool _isReady = false;

    public bool IsReady
        => Task.Run(async () =>
        {
            try
            {
                await _semaphore.WaitAsync();
                return _isReady;
            }
            finally { _semaphore.Release(); }
        }).GetAwaiter().GetResult();

    private async Task Validate()
    {
        try
        {
            await _semaphore.WaitAsync();
            _isReady = true;
        }
        finally { _semaphore.Release(); }
    }

    private async Task Invalidate()
    {
        try
        {
            await _semaphore.WaitAsync();
            _isReady = false;
        }
        finally { _semaphore.Release(); }
    }
}