using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers.StringHelpers;
using TTX.Services.StorageLayer.TagDatabase;

namespace TTX.Services.ApiLayer.TagData;

public partial class TagDataService : ITagDataService
{
    private readonly ITagDatabaseService _tagDatabase;
    private readonly ILogger<TagDataService> _logger;

    private readonly Random _random = new();

    public TagDataService(
        ITagDatabaseService tagDatabase,
        ILogger<TagDataService> logger)
    {
        _tagDatabase = tagDatabase;
        _logger = logger;
    }

    public async Task<TagCardResponse> GetCards(TagCardRequest request, CancellationToken ctoken = default)
    {
        Dictionary<string, TagCardState?> responseData = new();

        await _tagDatabase.Write(async (recs, ct) =>
        {
            ct.ThrowIfCancellationRequested();

            // Snapshot
            List<TagRecord> snapshot = await recs.ToListAsync(ctoken).ConfigureAwait(false);
            bool updateDb = false;
            Dictionary<string, TagCardState?> results = new();

            // Isolate records whose IDs are contained in the requested list
            TagRecord[] relevant = snapshot
                .Where(x => request.TagIds.Contains(x.TagId, StringComparer.OrdinalIgnoreCase))
                .ToArray();

            // Finish
            foreach (string tagId in request.TagIds)
            {
                TagRecord[] matches = relevant
                    .Where(x => x.TagId.Equals(tagId, StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                TagRecord? result = null;

                // Exactly one
                if (matches.Length == 1)
                {
                    result = matches[0];
                    continue;
                }

                // Too few
                if (matches.Length < 1)
                {
                    TagRecord newTag = GenerateTagRecord(tagId);
                    recs.Add(newTag);
                    result = newTag;
                    updateDb = true;
                    continue;
                }

                // Too many
                if (matches.Length > 1)
                {
                    _logger.LogError("Too many ({count}) records found for the tag: {id}", matches.Length, tagId);
                    continue;
                }

                results.Add(tagId, result?.ToState());
            }

            responseData = results;
            return updateDb;
        }, ctoken).ConfigureAwait(false);

        return new(responseData);
    }

    private TagRecord GenerateTagRecord(string key) => new()
    {
        TagId = key,
        Title = key.ToTitleFormat(),
        Color = RandomColor(100, 200),
    };

    private string RandomColor(byte lowerLimit, byte upperLimit)
    {
        int colorCount = 3;
        int skip = _random.Next(0, colorCount * 1000) % colorCount;

        StringBuilder result = new("#");

        for (int i = 0; i < colorCount; i++)
        {
            result.Append(i == skip
                ? 0.ToString("X2")
                : result.Append(((byte)_random.Next(lowerLimit, upperLimit)).ToString("X2")));
        }

        return result.ToString();
    }
}