using System.Collections.Generic;

namespace TTX.Data.Shared.QueryObjects;

public record TaggingRequest(string[] ItemIds, string TagId, bool Untag = false);
public record TaggingResponse(Dictionary<string, string[]> Updates, string[] Failures);