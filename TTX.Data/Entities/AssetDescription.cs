using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace TTX.Data.Entities;

public class AssetDescription
{
    public int ID { get; set; } = 0;
    public byte[]? UUID { get; set; } = null;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TagsString { get; set; } = string.Empty;
    public DateTime Added { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;

    [NotMapped]
    [JsonIgnore]
    public HashSet<string> Tags
    {
        get => TagsString.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet();
        set => TagsString = string.Join(' ', value);
    }
}