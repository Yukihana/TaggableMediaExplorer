using System.Threading;
using System.Threading.Tasks;
using TTX.Services.Legacy.TagsIndexer;

namespace TTX.Services.TagsIndexer;

/// <summary>
/// Storage service for TagInfo entities.
/// </summary>
public partial class TagsIndexerService : ITagsIndexerService
{
    private readonly TagsIndexerOptions _options;

    public TagsIndexerService(IWorkspaceProfile profile)
    {
        _options = profile.InitializeServiceOptions<TagsIndexerOptions>();
    }

    public async Task Reload(CancellationToken token = default)
    {
        await Purge().ConfigureAwait(false);
        await LoadRecords().ConfigureAwait(false);
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