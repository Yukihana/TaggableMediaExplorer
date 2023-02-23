namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    public partial int CacheCount()
        => ReadCacheSafely(() => _cache.Count);
}