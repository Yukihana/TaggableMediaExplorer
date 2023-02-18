using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.AssetsIndexer;

public interface IAssetsIndexerService
{
    // Api

    bool IsReady { get; }

    TOutput PerformQuery<TInput, TOutput>(TInput input, Func<TInput, IEnumerable<AssetRecord>, TOutput> func);

    // Control panel

    Task StartIndexing(CancellationToken token = default);

    Task StopIndexing(CancellationToken token = default);
}