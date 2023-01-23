using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.AssetsIndexer;

public interface IAssetsIndexerService
{
    bool IsReady { get; }

    TOutput PerformQuery<TInput, TOutput>(TInput input, Func<TInput, IEnumerable<AssetRecord>, TOutput> func);

    // Control panel

    Task StartIndexing(CancellationToken token = default);

    Task StopIndexing(CancellationToken token = default);

    // FSW

    void OnWatcherEvent(object sender, FileSystemEventArgs e);

    void OnWatcherEvent(object sender, RenamedEventArgs e);
}