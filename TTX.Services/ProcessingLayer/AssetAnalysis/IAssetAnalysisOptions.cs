namespace TTX.Services.ProcessingLayer.AssetAnalysis;

public interface IAssetAnalysisOptions : IServiceOptions
{
    int ReadBufferSize { get; set; }

    long SmallComputeMaximumSize { get; set; }

    int HashProcessingConcurrency { get; set; }

    int HashIOConcurrency { get; set; }

    int MetadataConcurrency { get; set; }

    int CrumbsCount { get; set; }
}