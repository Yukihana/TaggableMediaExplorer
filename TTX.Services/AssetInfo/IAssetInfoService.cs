using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.AssetInfo;

public interface IAssetInfoService
{
    Task<AssetFile?> Fetch(string path, CancellationToken token = default);

    Task<AssetFile?> FetchHashed(string path, CancellationToken token = default);

    Task<bool> ComputeHash(AssetFile file, CancellationToken token = default);

    Task ComputeHash(IEnumerable<AssetFile> files, CancellationToken token = default);

    // Legacy

    Task<byte[]?> ComputeSHA256Async(AssetFile file, CancellationToken token = default);

    Task<Dictionary<AssetFile, byte[]?>> ComputeSHA256Async(IEnumerable<AssetFile> paths, CancellationToken token = default);

    Task<AssetFile?> Fetch(string path, CancellationToken token = default);

    Task<List<AssetFile>> Fetch(IEnumerable<string> path, CancellationToken token = default);
}