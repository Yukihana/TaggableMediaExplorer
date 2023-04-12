using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TTX.Client.ViewContext;

namespace TTX.Client.ViewData;

public partial class BrowserData : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ItemCount))]
    private ObservableCollection<AssetCardContext> _items = new();

    // Single Selection

    [ObservableProperty]
    private int _selectedIndex = 0;

    [ObservableProperty]
    private AssetCardContext? _selectedItem = null;

    // Multiselect

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedCount))]
    private ObservableCollection<AssetCardContext> _selectedItems = new();

    // Derived Properties

    public int ItemCount => Items.Count;

    public int SelectedCount => SelectedItems.Count;
}