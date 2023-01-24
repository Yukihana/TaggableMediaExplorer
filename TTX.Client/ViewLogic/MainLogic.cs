using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.ViewLogic;

public partial class MainLogic : ObservableObject
{
    [ObservableProperty]
    public BrowserLogic _browserLogic = new();

    public async Task GuiLoaded(CancellationToken token = default)
    {
        await BrowserLogic.DoSearch("", token).ConfigureAwait(false);
    }
}