using System.Threading.Tasks;

namespace TTX.Services.AssetsIndexer;

public interface IAssetsIndexerService
{
    bool IsReady { get; }

    Task Reload();
}