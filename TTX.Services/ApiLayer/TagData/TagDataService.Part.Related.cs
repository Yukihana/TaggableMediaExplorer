using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Services.ApiLayer.TagData;

public partial class TagDataService
{
    public async Task<RelatedTagsResponse> GetRelatedTags(string searchText, CancellationToken ctoken = default)
    {
        string[] related = Array.Empty<string>();

        // Todo make a 'state' version of this.
        // fix this and all similar methods to use 'state' to improve performance
        await _tagDatabase.Read(async (recs, ct) =>
        {
            // snapshot
            TagRecord[] snapshot = await recs.ToArrayAsync(ct).ConfigureAwait(false);

            related = snapshot
                .Where(x => x.TagId.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.TagId).ToArray();
        }, ctoken).ConfigureAwait(false);

        return new(related);
    }
}