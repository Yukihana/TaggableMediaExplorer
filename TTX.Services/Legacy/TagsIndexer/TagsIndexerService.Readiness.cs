using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.TagsIndexer;

public partial class TagsIndexerService
{
    private readonly SemaphoreSlim _semReady = new(1);
    private bool _isReady = false;

    public bool IsReady
        => Task.Run(async () =>
        {
            try
            {
                await _semReady.WaitAsync();
                return _isReady;
            }
            finally { _semReady.Release(); }
        }).GetAwaiter().GetResult();

    private async Task Validate()
    {
        try
        {
            await _semReady.WaitAsync();
            _isReady = true;
        }
        finally { _semReady.Release(); }
    }

    private async Task Invalidate()
    {
        try
        {
            await _semReady.WaitAsync();
            _isReady = false;
        }
        finally { _semReady.Release(); }
    }
}