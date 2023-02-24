namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public interface IAssetSynchronisationOptions : IServiceOptions
{
    public string ServerRoot { get; set; }
    public string AssetsPath { get; set; }
}