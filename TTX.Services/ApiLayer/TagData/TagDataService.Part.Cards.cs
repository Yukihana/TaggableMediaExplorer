using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData.Entities;
using TTX.Data.SharedData.QueryObjects;

namespace TTX.Services.ApiLayer.TagData;

public partial class TagDataService
{
    public async Task<TagCardResponse> GetCards(TagCardRequest request, CancellationToken ctoken = default)
    {
        // Prepare result holders
        List<TagCardState> successes = new();
        HashSet<string> failures = new(request.TagIds, StringComparer.OrdinalIgnoreCase); // TODO add comparer here

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

                // Too many
                if (matches.Length > 1)
                {
                    _logger.LogError("Too many ({count}) records found for the tag: {id}", matches.Length, tagId);
                    continue;
                }

                // Exactly one
                else if (matches.Length == 1)
                    result = matches[0];

                // Too few
                else if (matches.Length < 1)
                {
                    TagRecord newTag = GenerateTagRecord(tagId);
                    recs.Add(newTag);
                    result = newTag;
                    updateDb = true;
                }

                // Append to results
                if (result is not null)
                {
                    failures.Remove(tagId);
                    successes.Add(result.ToState());
                }
            }

            return updateDb;
        }, ctoken).ConfigureAwait(false);

        return new(
            Results: successes.ToArray(),
            Failures: failures.ToArray());
    }
}