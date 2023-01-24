using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TTX.Client.ViewLogic;

namespace TTX.Client.ViewData;

public partial class BrowserData : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<AssetLogic> _items = new();
}