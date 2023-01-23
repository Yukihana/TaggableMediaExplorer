using System;

namespace TTX.Data.QueryObjects;

public class SearchResponse
{
    public string[] Results { get; set; } = Array.Empty<string>();
    public int TotalResults { get; set; } = 0;
    public int StartIndex { get; set; } = 0;
    public int EndIndex { get; set; } = 0;
}