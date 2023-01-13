using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.Metadata;

public interface IMetadataService
{
    Task<AssetFile?> Fetch(string path, CancellationToken token = default);

    Task<List<AssetFile>> Fetch(IEnumerable<string> path, CancellationToken token = default);
}