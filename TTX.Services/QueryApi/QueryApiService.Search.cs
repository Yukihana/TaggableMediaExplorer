using System;
using System.Collections.Generic;
using System.Linq;
using TTX.Data.QueryObjects;

namespace TTX.Services.QueryApi;

public partial class QueryApiService
{
    public SearchResponse Search(SearchQuery query)
    {
        var results = _assetsIndexer.PerformQuery(query.Keywords,
            (keywords, list) => list.Where(x =>
            {
                foreach (string word in keywords.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!x.Name.Contains(word, StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                return true;
            }));

        List<string> section = results
            .Skip(query.Page * query.Count)
            .Take(query.Count)
            .Select(x => new Guid(x.GUID).ToString())
            .ToList();

        return new()
        {
            Results = section.ToArray(),
            TotalResults = results.Count(),
            StartIndex = query.Page * query.Count,
            EndIndex = query.Page * query.Count + section.Count
        };
    }
}