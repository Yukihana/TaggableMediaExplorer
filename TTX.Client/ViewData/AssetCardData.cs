using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TTX.Client.ViewData;

public partial class AssetCardData : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ItemIdString))]
    private byte[] _itemId = Array.Empty<byte>();

    [ObservableProperty]
    private string _name = string.Empty;

    // Metadata

    [ObservableProperty]
    private HashSet<string> _tags = new();

    [ObservableProperty]
    private DateTime _addedUtc = DateTime.UtcNow;

    [ObservableProperty]
    private long _sizeBytes = 0;

    // Codec Information

    [ObservableProperty]
    private TimeSpan _duration = TimeSpan.Zero;

    [ObservableProperty]
    private uint _height = 0;

    [ObservableProperty]
    private uint _width = 0;

    // Derived

    [JsonIgnore]
    public string ItemIdString => ItemId.Length == 16 ? new Guid(ItemId).ToString() : string.Empty;

    // Gui use

    [ObservableProperty]
    [JsonIgnore]
    private bool _isSelected = false;
}