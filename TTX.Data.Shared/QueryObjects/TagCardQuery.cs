using System.Collections.Generic;

namespace TTX.Data.Shared.QueryObjects;

public record TagCardRequest(string[] TagIds, bool AutoAdd = true);
public record TagCardResponse(Dictionary<string, TagCardState?> Results);