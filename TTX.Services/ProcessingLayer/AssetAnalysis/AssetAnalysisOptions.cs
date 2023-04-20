using System;

namespace TTX.Services.ProcessingLayer.AssetAnalysis;

internal class AssetAnalysisOptions : IAssetAnalysisOptions
{
    public int ReadBufferSize { get; set; } = 4 * 1024 * 1024; // 4 MiB
    public long SmallComputeMaximumSize { get; set; } = 100 * 1024 * 1024; // 100 MiB
    public int HashProcessingConcurrency { get; set; } = 4;
    public int HashIOConcurrency { get; set; } = 1;
    public int MetadataConcurrency { get; set; } = 4;
    public int CrumbsCount { get; set; } = 16;

    // Init

    public void Initialize()
    { }

    public void Initialize(IRuntimeConfig runtimeConfig)
        => throw new NotImplementedException();
}