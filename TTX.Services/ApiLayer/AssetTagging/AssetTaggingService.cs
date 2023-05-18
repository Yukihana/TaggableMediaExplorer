using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.FileSystemHelpers;
using TTX.Library.Helpers.EnumerableHelpers;
using TTX.Library.Helpers.StringHelpers;
using TTX.Services.ProcessingLayer.AssetMetadata;
using TTX.Services.StorageLayer.AssetDatabase;
using TTX.Services.StorageLayer.AssetPresence;

namespace TTX.Services.ApiLayer.AssetTagging;

public partial class AssetTaggingService : IAssetTaggingService
{
    private readonly IAssetDatabaseService _assetDatabase;
    private readonly IAssetPresenceService _assetPresence;
    private readonly IAssetMetadataService _assetMetadata;

    private readonly ILogger<AssetTaggingService> _logger;

    public AssetTaggingService(
        IAssetDatabaseService assetDatabase,
        IAssetPresenceService assetPresence,
        IAssetMetadataService assetMetadata,
        ILogger<AssetTaggingService> logger)
    {
        _assetDatabase = assetDatabase;
        _assetPresence = assetPresence;
        _assetMetadata = assetMetadata;
        _logger = logger;
    }

    public async Task<TaggingResponse> Apply(TaggingRequest request, CancellationToken ctoken = default)
    {
        ctoken.ThrowIfCancellationRequested();
        if (!request.ItemIds.Any())
            return new(new(), Array.Empty<string>());

        // Prepare inputs and outputs
        Dictionary<string, bool> tags = new() { { request.TagId, request.Untag } };
        List<(string ItemIdString, byte[] ItemId, Dictionary<string, bool> Tags)> inputs
            = request.ItemIds
            .Select(x => (x, new Guid(x).ToByteArray(), tags))
            .ToList();

        ConcurrentBag<string> failures = new();
        ConcurrentDictionary<string, string[]> successes = new();

        // Evaluate in parallel
        await Parallel.ForEachAsync(inputs, ctoken, async (input, ct) =>
        {
            try
            {
                string[] tags = await ChangeTags(input.ItemId, input.Tags, ct).ConfigureAwait(false);
                successes.TryAdd(input.ItemIdString, tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed trying to apply tag {tags} to item {item}.", input.Tags, input.ItemIdString);
                failures.Add(input.ItemIdString);
            }
        }).ConfigureAwait(false);

        return new(
            Updates: successes.ToDictionary(x => x.Key, x => x.Value),
            Failures: failures.ToArray());
    }

    /// <summary>
    /// Apply tag changes to an asset record.
    /// </summary>
    /// <param name="tagChanges">Key: TagId, Value: Untag</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<string[]> ChangeTags(byte[] itemId, Dictionary<string, bool> tagChanges, CancellationToken ctoken = default)
    {
        if (_assetPresence.GetFirst(itemId) is not string localPath)
            throw new InvalidOperationException($"Item {new Guid(itemId)} does not have an active presence.");
        List<string> result = new();

        await _assetDatabase.WriteAsync(async (recs, ct) =>
        {
            try
            {
                ct.ThrowIfCancellationRequested();
                bool update = false;

                List<AssetRecord> snapshot = await recs.ToListAsync(ct).ConfigureAwait(false);
                AssetRecord match = snapshot
                    .Where(x => x.ItemId.SequenceEqual(itemId) &&
                        x.LocalPath.Equals(localPath, PlatformNamingHelper.FilenameComparison))
                    .SelectOneOrThrow($"Failed to select item to update tags. Reason: ");

                HashSet<string> tags = _assetMetadata.GetTagsAsHashSet(match);
                foreach (var item in tagChanges)
                {
                    string tag = item.Key.ToTagFormat();
                    if (item.Value ? tags.Remove(tag) : tags.Add(tag))
                        update = true;
                }

                if (update)
                    match.Tags = string.Join(' ', tags);

                result.AddRange(tags);
                return update;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an error trying to add a tag to item {id}.", new Guid(itemId));
                return false;
            }
        }, ctoken).ConfigureAwait(false);

        return result.ToArray();
    }
}