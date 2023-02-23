using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.Legacy.TagsIndexer;

public interface ITagsIndexerService
{
    Task Reload(CancellationToken token = default);
}