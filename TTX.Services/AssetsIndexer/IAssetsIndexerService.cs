using System.Threading.Tasks;

namespace TTX.Services.Indexer;

public interface IAssetsIndexerService
{
    Task Reload();
}