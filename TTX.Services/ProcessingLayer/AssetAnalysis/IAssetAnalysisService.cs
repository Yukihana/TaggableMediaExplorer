using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData.Models;

namespace TTX.Services.ProcessingLayer.AssetAnalysis;

public interface IAssetAnalysisService
{
    Task<bool> FileExists(string path, CancellationToken token = default);

    Task<QuickAssetSyncInfo?> Fetch(string path, string relativeTo, CancellationToken token = default);

    Task<FullAssetSyncInfo?> FetchHashed(string path, string relativeTo, CancellationToken token = default);
}