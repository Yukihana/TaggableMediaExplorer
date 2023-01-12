using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TTX.Data.Messages;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    public async Task Reload()
    {
        await Invalidate();
        await Purge();
        await LoadRecords();
        await ScanFiles();
        await Validate();
    }

    private async Task LoadRecords()
    {
        throw new NotImplementedException();
    }

    private async Task ScanFiles(CancellationToken token = default)
    {
        HashSet<string> paths = await _watcher.GetAllFiles(token);
        List<AssetFile> files = await _metadata.Fetch(paths, token);

        throw new NotImplementedException();
    }

    private async Task Purge()
    {
        throw new NotImplementedException();
    }
}