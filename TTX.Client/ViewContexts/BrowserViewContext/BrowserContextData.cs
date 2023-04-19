using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TTX.Client.ViewContexts.BrowserViewContext;

public partial class BrowserContextData : ObservableObject
{
    // Items

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ItemCount))]
    private ObservableCollection<AssetCardContext> _items = new();

    public int ItemCount => Items.Count;

    // Single Selection

    [ObservableProperty]
    private int _selectedIndex = 0;

    [ObservableProperty]
    private AssetCardContext? _selectedItem = null;

    // Multiselect

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedCount))]
    private AssetCardContext[] _selectedItems = Array.Empty<AssetCardContext>();

    public int SelectedCount => SelectedItems.Length;

    // Search query

    [ObservableProperty]
    private string _keywords = string.Empty;

    [ObservableProperty]
    private int _itemMax = 30;

    [ObservableProperty]
    private int _pageIndex = 0;
}