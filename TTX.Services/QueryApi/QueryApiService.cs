using TTX.Services.AssetsIndexer;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.QueryApi;

/// <summary>
/// Implementation for primary In-Memory Db Storage and web request handler.
/// </summary>
public partial class QueryApiService : IQueryApiService
{
    private readonly IAssetPresenceService _assetPresence;
    private readonly IAssetsIndexerService _assetsIndexer;

    public QueryApiService(IAssetPresenceService assetPresence, IAssetsIndexerService assetsIndexer)
    {
        _assetPresence = assetPresence;
        _assetsIndexer = assetsIndexer;
    }

    public int? AddTags(string uuid, string[] tags)
    {
        throw new System.NotImplementedException();
    }

    public string? GetDescription(string uuid)
    {
        throw new System.NotImplementedException();
    }

    public byte[]? GetFileSection(string uuid, long start, int length)
    {
        throw new System.NotImplementedException();
    }

    public string? GetIntegrityInfo(string uuid)
    {
        throw new System.NotImplementedException();
    }

    public string? GetMediaInfo(string uuid)
    {
        throw new System.NotImplementedException();
    }

    public string? GetMetadata(string uuid)
    {
        throw new System.NotImplementedException();
    }

    public string? GetTags(string uuid)
    {
        throw new System.NotImplementedException();
    }

    public byte[]? GetThumbnail(string uuid)
    {
        throw new System.NotImplementedException();
    }

    public int? RemoveTags(string uuid, string[] tags)
    {
        throw new System.NotImplementedException();
    }
}