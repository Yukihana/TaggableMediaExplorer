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

    public async Task Reload()
    {
        await Invalidate();
        await Purge();
        await LoadRecords();
        await Validate();
    }

    private async Task LoadRecords()
    {
    }

    private async Task Purge()
    {
    }
}