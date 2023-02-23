namespace TTX.Services.StorageLayer.AssetDatabase;

public class AssetDatabaseOptions : IAssetDatabaseOptions
{
    public bool EnableAssetDeletion { get; set; } = false;

    public void Initialize()
    { }
}