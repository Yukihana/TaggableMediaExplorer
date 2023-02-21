using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TTX.Library.FileSystemHelpers;

namespace TTX.Services.StorageLayer.AssetPresence;

// Add option to check if file exists?
// if so, will need to add options: ServerRoot, AssetsPath/Full, VerifyPresence(bool)

public class AssetPresenceService : IAssetPresenceService
{
    private readonly ILogger<AssetPresenceService> _logger;

    private readonly ConcurrentDictionary<string, byte[]> _presenceList;

    public AssetPresenceService(ILogger<AssetPresenceService> logger)
    {
        _logger = logger;

        _presenceList = new(PlatformNamingHelper.FilenameComparer);
    }

    public void Clear()
    {
        _logger.LogWarning("Clearing asset presence. No of entries: {count}", _presenceList.Count);
        _presenceList.Clear();
    }

    public byte[]? Get(string localPath)
        => _presenceList.GetValueOrDefault(localPath);

    public string? GetFirst(byte[] itemId)
    {
        IEnumerable<KeyValuePair<string, byte[]>> matched = _presenceList.Where(x => x.Value.SequenceEqual(itemId));
        return matched.Any() ? matched.First().Key : null;
    }

    public string[] GetAll(byte[] itemId)
        => _presenceList.Where(x => x.Value.SequenceEqual(itemId)).Select(x => x.Key).ToArray();

    public void Remove(string localPath)
    {
        if (_presenceList.TryRemove(localPath, out byte[]? itemId))
            _logger.LogWarning("Unregistering asset presence: \"{path}\". ID: \"{itemId}\".", localPath, itemId);
    }

    public void RemoveAll(byte[] itemId)
    {
        foreach (string key in GetAll(itemId))
        {
            _logger.LogWarning("Removing all presences with ID {itemId}: {localPath}", itemId, key);
            _presenceList.Remove(key, out _);
        }
    }

    public void Set(string localPath, byte[] itemId)
    {
        if (_presenceList.TryGetValue(localPath, out byte[]? oldId))
            _logger.LogWarning("Updating asset presence: \"{path}\". Old: \"{oldId}\". New: \"{newId}\".",
                localPath, new Guid(oldId), new Guid(itemId));
        else
            _logger.LogInformation("Registering asset presence: \"{path}\". ID: \"{newId}\".", localPath, new Guid(itemId));
        _presenceList[localPath] = itemId;
    }
}