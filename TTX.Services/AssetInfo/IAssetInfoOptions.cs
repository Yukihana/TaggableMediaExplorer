namespace TTX.Services.AssetInfo;

public interface IAssetInfoOptions : IServiceOptions
{
    int ReadBufferSize { get; set; }

    long SmallComputeMaximumSize { get; set; }

    int HashProcessingConcurrency { get; set; }

    int HashIOConcurrency { get; set; }

    int MetadataConcurrency { get; set; }

    int CrumbsCount { get; set; }
}