namespace TTX.Services.QueryApi;

public class QueryResponse
{
    public object? Data { get; set; } = null;
    public QueryStatus Status { get; set; } = QueryStatus.Unknown;
    public object? StatusParameter { get; set; } = null;
}