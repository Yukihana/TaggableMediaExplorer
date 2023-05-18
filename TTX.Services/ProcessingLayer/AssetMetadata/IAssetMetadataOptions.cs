namespace TTX.Services.ProcessingLayer.AssetMetadata;

public interface IAssetMetadataOptions : IServiceProfile
{
    //Tags

    bool IgnoreTagIdCasing { get; set; }
    char TagSeparator { get; set; }
}