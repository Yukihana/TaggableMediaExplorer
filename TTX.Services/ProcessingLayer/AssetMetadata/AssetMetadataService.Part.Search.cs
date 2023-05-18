using System;
using TTX.Data.ServerData.Entities;

namespace TTX.Services.ProcessingLayer.AssetMetadata;

public partial class AssetMetadataService
{
    public bool Contains(AssetRecord asset, string searchString)
        =>
        throw new System.NotImplementedException();

    public bool Contains(AssetRecord asset, string[] keywords)
    {
        foreach (string keyword in keywords)
        {
            if (asset.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                continue;
            if (HasTag(asset, keyword))
                continue;
            return false;
        }
        return true;
    }
}