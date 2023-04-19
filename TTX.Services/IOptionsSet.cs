using TTX.Services.ApiLayer.AssetContent;
using TTX.Services.ControlLayer.AssetIndexing;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.Legacy.TagsIndexer;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.ProcessingLayer.AssetSynchronisation;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPreview;

namespace TTX.Services;

public interface IOptionsSet :

    // Storage
    IAssetDatabaseOptions,
    IAssetPreviewOptions,

    // Processing
    IAssetAnalysisOptions,
    IAssetSynchronisationOptions,

    // Incoming
    IAssetTrackingOptions,

    // Control
    IAssetIndexingOptions,
    ITagsIndexerOptions,

    // Api
    IAssetContentOptions
{
}