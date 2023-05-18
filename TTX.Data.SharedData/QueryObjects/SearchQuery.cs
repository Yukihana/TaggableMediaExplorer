using TTX.Data.SharedData.QueryObjects;

namespace TTX.Data.Shared.QueryObjects;

public record SearchRequest(
    string Keywords = "",
    int Page = 0,
    int Count = 30);

public record SearchResponse(
    AssetCardState[] Results,
    int TotalResults = 0,
    int StartIndex = 0,
    int EndIndex = 0);