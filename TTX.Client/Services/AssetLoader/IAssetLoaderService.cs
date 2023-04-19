using System;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.AssetLoader;

internal interface IAssetLoaderService
{
    Task<string> GetCachedPath(string idString, DateTime lastUpdatedUtc, CancellationToken ctoken = default);
}