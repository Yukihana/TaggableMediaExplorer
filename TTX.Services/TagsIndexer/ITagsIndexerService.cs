using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.TagsIndexer;

public interface ITagsIndexerService
{
    Task Reload(CancellationToken token = default);
}