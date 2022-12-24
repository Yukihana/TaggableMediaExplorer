namespace TTX.Services.AssetsIndexer;

public interface IAssetsIndexerOptions : IServiceOptions
{
    string AssetsIndexerSID { get; set; }
}