using System.Threading;

namespace TTX.Services.TagsIndexer;

public partial class TagsIndexerService
{
    private readonly SemaphoreSlim _semReady = new(1);
}