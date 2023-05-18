using System.Collections.Generic;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Services.ProcessingLayer.AssetMetadata;

public interface IAssetMetadataService
{
    // Create state

    AssetCardState CreateAssetCardState(AssetRecord asset);

    // Search

    bool Contains(AssetRecord asset, string searchString);

    bool Contains(AssetRecord asset, string[] keywords);

    // Tags

    HashSet<string> GetTagsAsHashSet(AssetRecord asset);

    string[] GetTags(AssetRecord asset);

    void SetTags(AssetRecord asset, IEnumerable<string> tags);

    bool HasTag(AssetRecord asset, string tag);

    bool AddTag(AssetRecord asset, string tag);

    bool RemoveTag(AssetRecord asset, string tag);

    string[] ApplyTag(AssetRecord asset, string tag, bool untag);

    string[] ApplyTags(AssetRecord asset, Dictionary<string, bool> tags);
}