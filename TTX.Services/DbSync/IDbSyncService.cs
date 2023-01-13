using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.DbSync;

/// <summary>
/// The interface for handling database read/write operations.
/// </summary>
public interface IDbSyncService
{
    Task<List<AssetRecord>> LoadAssets(CancellationToken token = default);

    Task<List<TagRecord>> LoadTags(CancellationToken token = default);
}