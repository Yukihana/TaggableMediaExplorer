using CommunityToolkit.Mvvm.ComponentModel;
using TTX.Client.ViewLogic;

namespace TTX.Client.Services.MainGui;

public partial class MainData : ObservableObject
{
    // Components

    [ObservableProperty]
    private BrowserLogic _browserLogic = new();

    // Data

    [ObservableProperty]
    private string _keywords = string.Empty;

    [ObservableProperty]
    private int _itemMax = 30;

    [ObservableProperty]
    private int _pageIndex = 0;
}