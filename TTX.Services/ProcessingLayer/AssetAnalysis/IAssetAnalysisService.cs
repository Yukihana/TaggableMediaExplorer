using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Models;

namespace TTX.Services.ProcessingLayer.AssetAnalysis;

public interface IAssetAnalysisService
{
    Task<bool> FileExists(string path, CancellationToken token = default);

    Task<AssetQuickSyncInfo?> Fetch(string path, string relativeTo, CancellationToken token = default);

    Task<AssetFullSyncInfo?> FetchHashed(string path, string relativeTo, CancellationToken token = default);
}