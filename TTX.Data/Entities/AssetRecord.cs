using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;

namespace TTX.Data.Entities;

public class AssetRecord
{
    [Key]
    public int ID { get; set; } = 0;

    // Identity

    public byte[] ItemId { get; set; } = Array.Empty<byte>();
    public string FilePath { get; set; } = string.Empty;

    // Metadata

    public DateTime CreatedUtc { get; set; } = DateTime.Now;
    public DateTime ModifiedUtc { get; set; } = DateTime.Now;
    public long SizeBytes { get; set; } = 0;

    // Integrity

    public byte[] SHA256 { get; set; } = Array.Empty<byte>();
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();
    public string MatchIgnore { get; set; } = string.Empty;

    // Media Info

    public int MediaWidth { get; set; } = 0;
    public int MediaHeight { get; set; } = 0;
    public TimeSpan MediaDuration { get; set; } = TimeSpan.Zero;

    // Codecs

    public string Container { get; set; } = string.Empty;
    public string DefaultVideoTrackCodec { get; set; } = string.Empty;
    public string DefaultAudioTrackCodec { get; set; } = string.Empty;
    public string DefaultSubtitlesFormat { get; set; } = string.Empty;

    // User Data

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TagsString { get; set; } = string.Empty;
    public DateTime AddedUtc { get; set; } = DateTime.Now;
    public DateTime UpdatedUtc { get; set; } = DateTime.Now;

    // Not Mapped

    [NotMapped]
    [JsonIgnore]
    public HashSet<string> Tags
    {
        get => TagsString.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet();
        set => TagsString = string.Join(' ', value).ToLowerInvariant();
    }

    [NotMapped]
    [JsonIgnore]
    public HashSet<string> Duplicates { get; set; } = new();

    [NotMapped]
    [JsonIgnore]
    public bool IsValid { get; set; } = false;

    [NotMapped]
    [JsonIgnore]
    public ReaderWriterLockSlim Lock { get; set; } = new();
}