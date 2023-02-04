namespace TTX.Services.AssetsIndexer;

public interface IAssetsIndexerOptions : IServiceOptions
{
    public string ServerRoot { get; set; }
    public string AssetsPath { get; set; }
}