using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using TTX.Data.Models;

namespace TTX.Data.Entities;

public class AssetRecord : IAssetItemId, IAssetHashedMetadata
{
    [Key]
    public int ID { get; set; } = 0;

    // Identity

    public byte[] ItemId { get; set; } = Array.Empty<byte>();

    // Metadata

    public string LocalPath { get; set; } = string.Empty;
    public long SizeBytes { get; set; } = 0;
    public DateTime CreatedUtc { get; set; } = DateTime.Now;
    public DateTime ModifiedUtc { get; set; } = DateTime.Now;
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();

    // Hash Info

    public DateTime VerifiedUtc { get; set; } = DateTime.Now;
    public byte[] SHA256 { get; set; } = Array.Empty<byte>();

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

    // Undecided

    public string SimilarIgnore { get; set; } = string.Empty;

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
    public ReaderWriterLockSlim Lock { get; set; } = new();
}