namespace TTX.Services.Legacy.AssetsIndexer;

public interface IAssetsIndexerOptions : IServiceOptions
{
    public string ServerRoot { get; set; }
    public string AssetsPath { get; set; }
}