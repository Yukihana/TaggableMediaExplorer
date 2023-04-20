namespace TTX.Services.StorageLayer.AssetDatabase;

public interface IAssetDatabaseOptions : IServiceProfile
{
    bool EnableAssetDeletion { get; set; }
}