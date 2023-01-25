using CommunityToolkit.Mvvm.ComponentModel;
using TTX.Client.ViewData;

namespace TTX.Client.ViewLogic;

public partial class BrowserLogic : ObservableObject
{
    [ObservableProperty]
    private BrowserData _dataModel = new();

    // Startup

    public BrowserLogic()
    {
        DataModel.PropertyChanged += BrowserDataChanged;
    }

    private void BrowserDataChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // On page change : immediate dispatch
        // On text change : wait for ... wait, this goes into the menu bar
    }
}