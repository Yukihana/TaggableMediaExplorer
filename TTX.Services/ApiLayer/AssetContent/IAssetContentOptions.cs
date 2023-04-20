namespace TTX.Services.ApiLayer.AssetContent;

public interface IAssetContentOptions : IServiceProfile
{
    string AssetsPath { get; set; }
}