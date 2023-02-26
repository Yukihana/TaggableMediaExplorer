using TTX.Services.ControlLayer.AssetIndexing;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.Legacy.TagsIndexer;
using TTX.Services.Legacy.Thumbnails;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.ProcessingLayer.AssetSynchronisation;
using TTX.Services.StorageLayer.AssetDatabase;

namespace TTX.Services;

public interface IOptionsSet :

    // Storage
    IAssetDatabaseOptions,
    IThumbnailOptions,

    // Processing
    IAssetAnalysisOptions,
    IAssetSynchronisationOptions,

    // Incoming
    IAssetTrackingOptions,

    // Control
    IAssetIndexingOptions,
    ITagsIndexerOptions
{
}