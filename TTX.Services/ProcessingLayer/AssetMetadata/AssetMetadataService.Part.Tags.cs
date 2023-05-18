using System;
using System.Collections.Generic;
using System.Linq;
using TTX.Data.ServerData.Entities;

namespace TTX.Services.ProcessingLayer.AssetMetadata;

public partial class AssetMetadataService
{
    // Get as HashSet

    public HashSet<string> GetTagsAsHashSet(AssetRecord asset)
        => asset.Tags
        .Split(_options.TagSeparator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .ToHashSet(_options.TagComparer);

    // Get, Set, Has

    public string[] GetTags(AssetRecord asset)
        => GetTagsAsHashSet(asset).ToArray();

    public void SetTags(AssetRecord asset, IEnumerable<string> tags)
        => asset.Tags = string.Join(_options.TagSeparator, tags.ToHashSet(_options.TagComparer));

    public bool HasTag(AssetRecord asset, string tag)
        => GetTagsAsHashSet(asset).Contains(tag);

    // Add, Remove

    public bool AddTag(AssetRecord asset, string tag)
    {
        HashSet<string> current = GetTagsAsHashSet(asset);
        bool updated = current.Add(tag);

        if (updated)
            SetTags(asset, current);
        return updated;
    }

    public bool RemoveTag(AssetRecord asset, string tag)
    {
        HashSet<string> current = GetTagsAsHashSet(asset);
        bool updated = current.Remove(tag);

        if (updated)
            SetTags(asset, current);
        return updated;
    }

    // Apply

    public string[] ApplyTag(AssetRecord asset, string tag, bool untag)
    {
        HashSet<string> current = GetTagsAsHashSet(asset);
        bool updated = untag ? current.Remove(tag) : current.Add(tag);

        if (updated)
            SetTags(asset, current);
        return current.ToArray();
    }

    public string[] ApplyTags(AssetRecord asset, Dictionary<string, bool> tags)
    {
        HashSet<string> current = GetTagsAsHashSet(asset);
        bool updated = false;

        foreach (var kvp in tags)
        {
            if (kvp.Value ? current.Remove(kvp.Key) : current.Add(kvp.Key))
                updated = true;
        }

        if (updated)
            SetTags(asset, current);
        return current.ToArray();
    }
}