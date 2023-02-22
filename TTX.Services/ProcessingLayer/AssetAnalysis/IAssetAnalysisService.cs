using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.ProcessingLayer.AssetAnalysis;

public interface IAssetAnalysisService
{
    Task<bool> FileExists(string path, CancellationToken token = default);

    Task<AssetFile?> Fetch(string path, bool computeHash = false, CancellationToken token = default);

    Task<bool> ComputeHash(AssetFile file, CancellationToken token = default);
}