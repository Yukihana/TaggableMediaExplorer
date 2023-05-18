using System.Collections.Generic;

namespace TTX.Data.SharedData.QueryObjects;

public record AssetCardRequest(string[] IdStrings);
public record AssetCardResponse(Dictionary<string, AssetCardState?> Cards);