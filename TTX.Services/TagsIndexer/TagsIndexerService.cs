using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.TagsIndexer;

/// <summary>
/// Storage service for TagInfo entities.
/// </summary>
public partial class TagsIndexerService : ITagsIndexerService
{
    private readonly TagsIndexerOptions _options;

    public TagsIndexerService(IOptionsSet options)
    {
        _options = options.ExtractValues<TagsIndexerOptions>();
    }

    public async Task Reload(CancellationToken token = default)
    {
        await Invalidate().ConfigureAwait(false);
        await Purge().ConfigureAwait(false);
        await LoadRecords().ConfigureAwait(false);
        await Validate().ConfigureAwait(false);
    }

    private async Task LoadRecords()
    {
        await Task.Delay(1).ConfigureAwait(false);
    }

    private async Task Purge()
    {
        await Task.Delay(1).ConfigureAwait(false);
    }
}