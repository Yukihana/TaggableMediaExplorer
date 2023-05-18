using System.Collections.Generic;

namespace TTX.Data.Shared.QueryObjects;

public record AssetCardRequest(string[] IdStrings);
public record AssetCardResponse(Dictionary<string, AssetCardState?> Cards);