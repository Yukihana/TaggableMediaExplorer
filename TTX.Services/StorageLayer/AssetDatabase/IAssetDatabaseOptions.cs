namespace TTX.Services.StorageLayer.AssetDatabase;

public interface IAssetDatabaseOptions : IServiceOptions
{
    bool EnableAssetDeletion { get; set; }
}