using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace TTX.Client.ViewContexts.MediaViewContext;

public partial class MediaContextData : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<QueueItemContext> _items = new();

    [ObservableProperty]
    private QueueItemContext[] _selectedItems = Array.Empty<QueueItemContext>();

    [ObservableProperty]
    private QueueItemContext _activeMedia = new();

    [ObservableProperty]
    private string _controlCommandsPipe = string.Empty;

    [ObservableProperty]
    private string _controlEventsPipe = string.Empty;

    public MediaContextData()
    {
        Items.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(Items));
    }
}