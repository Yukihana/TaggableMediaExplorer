namespace TTX.Services.ApiLayer.AssetContent;

public interface IAssetContentOptions : IServiceOptions
{
    string ServerRoot { get; set; }
    string AssetsPath { get; set; }
}