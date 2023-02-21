namespace TTX.Services.StorageLayer.AssetPresence;

public interface IAssetPresenceService
{
    byte[]? Get(string localPath);

    string? GetFirst(byte[] itemId);

    string[] GetAll(byte[] itemId);

    void Set(string localPath, byte[] itemId);

    void Remove(string localPath);

    void RemoveAll(byte[] itemId);

    void Clear();
}