using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextLogic
{
    // todo make a master results setter coordinator method
    private async Task SetContexts(IEnumerable<AssetCardContext> results, CancellationToken ctoken = default)
    {
        ObservableCollection<AssetCardContext> collection = new(results);
        await _guiSync.DispatchActionAsync(
            state => ContextData.Items = state,
            collection, ctoken).ConfigureAwait(false);
    }
}