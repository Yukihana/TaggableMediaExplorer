using System;
using System.Collections.Generic;
using System.Linq;
using TTX.Data.Shared.QueryObjects;
using TTX.Services.AbstractionLayer.AssetQuery;

namespace TTX.Services.ApiLayer.AssetSearch;

public partial class AssetSearchService : IAssetSearchService
{
    private readonly IAssetQueryService _assetQuery;

    public AssetSearchService(IAssetQueryService assetQuery)
    {
        _assetQuery = assetQuery;
    }

    public SearchResponse Search(SearchQuery query)
    {
        var results = _assetQuery.PerformQuery(query.Keywords,
            (keywords, list) => list.Where(x =>
            {
                foreach (string word in keywords.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!x.Title.Contains(word, StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                return true;
            }));

        List<string> section = results
            .Skip(query.Page * query.Count)
            .Take(query.Count)
            .Select(x => new Guid(x.ItemId).ToString())
            .ToList();

        int total = results.Count();
        int shown = section.Count;

        return new()
        {
            Results = section.ToArray(),
            TotalResults = total,
            StartIndex = shown > 0 ? query.Page * query.Count : -1,
            EndIndex = shown > 0 ? query.Page * query.Count + section.Count - 1 : -1,
        };
    }
}