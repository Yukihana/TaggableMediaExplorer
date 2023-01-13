using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.Integrity;

public interface IIntegrityService
{
    Task<byte[]> GetCrumbsAsync(string path, CancellationToken token = default);
}