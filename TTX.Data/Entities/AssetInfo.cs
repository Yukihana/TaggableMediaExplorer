using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace TTX.Data.Entities;

public class AssetInfo
{
    public int ID { get; set; } = 0;
    public byte[]? GUID { get; set; } = null;

    // Identity

    public string LastLocation { get; set; } = string.Empty;

    // Metadata

    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Modified { get; set; } = DateTime.Now;
    public long SizeBytes { get; set; } = 0;

    // Integrity

    public byte[]? SHA2 { get; set; } = null;
    public byte[]? FileCrumbs { get; set; } = null;

    // Media Info

    public int Height { get; set; } = 0;
    public int Width { get; set; } = 0;
    public int Length { get; set; } = 0;

    // Codecs

    public string Container { get; set; } = string.Empty;
    public string DefaultVideoTrackCodec { get; set; } = string.Empty;
    public string DefaultAudioTrackCodec { get; set; } = string.Empty;
    public string DefaultSubtitlesFormat { get; set; } = string.Empty;

    // Description

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TagsString { get; set; } = string.Empty;
    public DateTime Added { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;

    // Not Mapped

    [NotMapped]
    [JsonIgnore]
    public HashSet<string> Tags
    {
        get => TagsString.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet();
        set => TagsString = string.Join(' ', value);
    }
}