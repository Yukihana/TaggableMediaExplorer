using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.ViewData;

namespace TTX.Client.ViewLogic;

public partial class MainLogic : ObservableObject
{
    [ObservableProperty]
    private MainData _dataModel = new();

    public async Task GuiLoaded(CancellationToken token = default)
        => await SearchNew(string.Empty, token).ConfigureAwait(false);
}