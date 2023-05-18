using TTX.Services.ApiLayer.AssetContent;
using TTX.Services.IncomingLayer.AssetTracking;
using TTX.Services.Legacy.TagsIndexer;
using TTX.Services.ProcessingLayer.AssetAnalysis;
using TTX.Services.ProcessingLayer.AssetMetadata;
using TTX.Services.ProcessingLayer.AssetSynchronisation;
using TTX.Services.ProcessingLayer.MediaAnalysis;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPreview;

namespace TTX.Services;

public interface IWorkspaceProfile :

    // Storage
    IAssetDatabaseOptions,
    IAssetPreviewOptions,

    // Processing
    IAssetMetadataOptions,
    IAssetAnalysisOptions,
    IAssetSynchronisationOptions,
    IMediaAnalysisOptions,

    // Incoming
    IAssetTrackingOptions,

    // Control
    ITagsIndexerOptions,

    // Api
    IAssetContentOptions
{
}