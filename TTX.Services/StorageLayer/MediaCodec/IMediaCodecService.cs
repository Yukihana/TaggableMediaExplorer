using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData.Models;

namespace TTX.Services.StorageLayer.MediaCodec;

public interface IMediaCodecService
{
    Task<AssetMediaInfo> GetMediaAnalysisInfo(string sourcePath, CancellationToken ctoken = default);

    Task<bool> SnapshotAsync(string assetPath, string snapshotPath, TimeSpan captureTime, CancellationToken ctoken = default);
}