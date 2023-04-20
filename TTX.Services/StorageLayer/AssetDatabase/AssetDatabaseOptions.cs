using System;

namespace TTX.Services.StorageLayer.AssetDatabase;

public class AssetDatabaseOptions : IAssetDatabaseOptions
{
    public bool EnableAssetDeletion { get; set; } = false;

    // Init

    public void Initialize()
    { }

    public void Initialize(IRuntimeConfig runtimeConfig)
        => throw new NotImplementedException();
}