using System.Collections.Generic;

namespace TTX.Data.SharedData.QueryObjects;

public record TagCardRequest(string[] TagIds, bool AutoAdd = true);
public record TagCardResponse(Dictionary<string, TagCardState?> Results);