using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TTX.Library.Helpers;

namespace TTX.Client.ViewData;

public partial class AssetCardData : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ItemIdString))]
    private byte[] _itemId = Array.Empty<byte>();

    // Metadata

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SizeString))]
    private long _sizeBytes = 0;

    // Codec Information

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DurationConcise))]
    private TimeSpan _mediaDuration = TimeSpan.Zero;

    [ObservableProperty]
    private uint _mediaWidth = 0;

    [ObservableProperty]
    private uint _mediaHeight = 0;

    // User Data

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private HashSet<string> _tags = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AddedDateDiff))]
    private DateTime _addedUtc = DateTime.UtcNow;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UpdatedDateDiff))]
    private DateTime _updatedUtc = DateTime.UtcNow;

    // ------- //
    // Derived //
    // ------- //

    [JsonIgnore]
    public string ItemIdString => ItemId.Length == 16 ? new Guid(ItemId).ToString() : string.Empty;

    // Derived : Metadata

    [JsonIgnore]
    public string SizeString => SizeBytes.ToSizeString();

    [JsonIgnore]
    public string AddedDateDiff => AddedUtc.GetTimeDiff();

    [JsonIgnore]
    public string UpdatedDateDiff => UpdatedUtc.GetTimeDiff();

    // Derived : Codec

    [JsonIgnore]
    public string DurationConcise => MediaDuration.ToConciseDuration();

    // Gui use

    [ObservableProperty]
    [JsonIgnore]
    private bool _isSelected = false;

    [ObservableProperty]
    [JsonIgnore]
    private string _thumbPath = string.Empty;
}