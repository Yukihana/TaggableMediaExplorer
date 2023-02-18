using TTX.Services.AssetInfo;
using TTX.Services.AssetsIndexer;
using TTX.Services.Auxiliary;
using TTX.Services.DbSync;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.TagsIndexer;
using TTX.Services.Thumbnails;

namespace TTX.Services;

public interface IOptionsSet :
    IAssetInfoOptions,
    IAssetsIndexerOptions,
    IAuxiliaryOptions,
    IDbSyncOptions,
    ITagsIndexerOptions,
    IThumbnailOptions,
    IAssetTrackingOptions
{
}