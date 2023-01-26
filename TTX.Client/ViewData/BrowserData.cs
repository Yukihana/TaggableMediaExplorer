using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TTX.Client.ViewLogic;

namespace TTX.Client.ViewData;

public partial class BrowserData : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ItemCount))]
    private ObservableCollection<AssetCardLogic> _items = new();

    // Single Selection

    [ObservableProperty]
    private int _selectedIndex = 0;

    [ObservableProperty]
    private AssetCardLogic? _selectedItem = null;

    // Multiselect

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ItemCount))]
    private ObservableCollection<int> _selectedIndices = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedCount))]
    private ObservableCollection<AssetCardLogic> _selectedItems = new();

    // Derived Properties

    public int ItemCount => Items.Count;

    public int SelectedCount => SelectedItems.Count;

    // Ctor
}