using System;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.InstancingHelpers;

namespace TTX.Services.ProcessingLayer.AssetMetadata;

public partial class AssetMetadataService
{
    public AssetCardState CreateAssetCardState(AssetRecord asset)
    {
        AssetCardState result = new()
        {
            ItemId = new Guid(asset.ItemId).ToString(),
            Tags = GetTags(asset),
        };
        asset.CopyPropertiesTo(result);
        return result;
    }
}