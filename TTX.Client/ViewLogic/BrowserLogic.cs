using CommunityToolkit.Mvvm.ComponentModel;
using TTX.Client.ViewData;

namespace TTX.Client.ViewLogic;

public partial class BrowserLogic : ObservableObject
{
    [ObservableProperty]
    private BrowserData _dataModel = new();
}