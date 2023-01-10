using System.Threading.Tasks;

namespace TTX.Services.TagsIndexer;

public interface ITagsIndexerService
{
    Task Reload();
}