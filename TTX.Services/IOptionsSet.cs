using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.Legacy.AssetsIndexer;
using TTX.Services.Legacy.Auxiliary;
using TTX.Services.Legacy.DbSync;
using TTX.Services.Legacy.TagsIndexer;
using TTX.Services.Legacy.Thumbnails;
using TTX.Services.ProcessingLayer.AssetAnalysis;

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