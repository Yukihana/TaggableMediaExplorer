using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using TTX.Client.ViewData;

namespace TTX.Client.ViewLogic;

public partial class MainLogic : ObservableObject
{
    [ObservableProperty]
    private MainData _dataModel = new();

    public void GuiLoaded()
        => _ = Task.Run(async () => await SearchNew(string.Empty, SessionContext.CancellationToken).ConfigureAwait(false));

    public void GuiClosed()
        => SessionContext.QueryClose(this);
}