using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using TTX.Library.Helpers;

namespace TTX.Client.ViewData;

public partial class AssetCardData : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ItemIdString))]
    private byte[] _itemId = Array.Empty<byte>();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FileName))]
    private string _filePath = string.Empty;

    // Metadata

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SizeString))]
    private long _sizeBytes = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AddedDateDiff))]
    private DateTime _addedUtc = DateTime.UtcNow;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UpdatedDateDiff))]
    private DateTime _updatedUtc = DateTime.UtcNow;

    // Codec Information

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DurationConcise))]
    private TimeSpan _mediaDuration = TimeSpan.Zero;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MediaResolution))]
    private uint _mediaWidth = 0;

    [ObservableProperty]
    private uint _mediaHeight = 0;

    // User Data

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private HashSet<string> _tags = new();

    // ------- //
    // Derived //
    // ------- //

    [JsonIgnore]
    public string ItemIdString => ItemId.Length == 16 ? new Guid(ItemId).ToString() : string.Empty;

    [JsonIgnore]
    public string FileName => Path.GetFileName(FilePath) ?? string.Empty;

    // Derived : Metadata

    [JsonIgnore]
    public string SizeString => SizeBytes.ToSizeString();

    [JsonIgnore]
    public string AddedDateDiff => AddedUtc.GetTimeDiff(true);

    [JsonIgnore]
    public string UpdatedDateDiff => UpdatedUtc.GetTimeDiff();

    // Derived : Codec

    [JsonIgnore]
    public string DurationConcise => MediaDuration.ToConciseDuration();

    [JsonIgnore]
    public string MediaResolution => $"{MediaWidth} x {MediaHeight}";

    // Gui use

    [ObservableProperty]
    [JsonIgnore]
    private bool _isSelected = false;

    [ObservableProperty]
    [JsonIgnore]
    private string _thumbPath = string.Empty;
}