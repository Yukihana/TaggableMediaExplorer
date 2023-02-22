using TTX.Services.AssetsIndexer;
using TTX.Services.Auxiliary;
using TTX.Services.DbSync;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.TagsIndexer;
using TTX.Services.Thumbnails;

namespace TTX.Services;

public interface IOptionsSet :
    IAssetAnalysisOptions,
    IAssetsIndexerOptions,
    IAuxiliaryOptions,
    IDbSyncOptions,
    ITagsIndexerOptions,
    IThumbnailOptions,
    IAssetTrackingOptions
{
}