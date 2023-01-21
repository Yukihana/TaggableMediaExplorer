﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;

namespace TTX.Data.Entities;

public class AssetRecord
{
    public int ID { get; set; } = 0;
    public byte[] GUID { get; set; } = Array.Empty<byte>();

    // Identity

    public string LastLocation { get; set; } = string.Empty;

    // Metadata

    public DateTime CreatedUtc { get; set; } = DateTime.Now;
    public DateTime ModifiedUtc { get; set; } = DateTime.Now;
    public long SizeBytes { get; set; } = 0;

    // Integrity

    public byte[] SHA2 { get; set; } = Array.Empty<byte>();
    public byte[] Crumbs { get; set; } = Array.Empty<byte>();
    public string[] MatchIgnore { get; set; } = Array.Empty<string>();

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

    [NotMapped]
    [JsonIgnore]
    public HashSet<string> Duplicates { get; set; } = new();

    [NotMapped]
    [JsonIgnore]
    public bool IsValid { get; set; } = false;

    [NotMapped]
    [JsonIgnore]
    public SemaphoreSlim Semaphore { get; set; } = new(1);
}