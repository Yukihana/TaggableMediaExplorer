using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTX.Data.Messages;
using TTX.Services.AssetsIndexer;
using TTX.Services.Watcher;

namespace TTX.Services.Indexer;

/// <summary>
/// Storage class for AssetInfo entities.
/// </summary>
public partial class AssetsIndexerService : IAssetsIndexerService
{
    private readonly IWatcherService _watcher;
    private readonly IAssetsIndexerOptions _options;

    public AssetsIndexerService(IWatcherService watcher, IAssetsIndexerOptions options)
    {
        _watcher = watcher;
        _options = options;
    }

    public async Task Reload()
    {
        await Invalidate();
        await Purge();
        await LoadRecords();
        await ScanFiles();
        await Validate();
    }

    private async Task Validate()
    {
        throw new NotImplementedException();
    }

    private async Task ScanFiles()
    {
        List<AssetFile> files = await _watcher.GetFiles();
        throw new NotImplementedException();
    }

    private async Task LoadRecords()
    {
        throw new NotImplementedException();
    }

    private async Task Purge()
    {
        throw new NotImplementedException();
    }

    private async Task Invalidate()
    {
        throw new NotImplementedException();
    }
}