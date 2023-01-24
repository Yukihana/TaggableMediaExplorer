namespace TTX.Data.Shared.QueryObjects;

public class SearchQuery
{
    public string Keywords { get; set; } = string.Empty;
    public int Page { get; set; } = 0;
    public int Count { get; set; } = 30;
}