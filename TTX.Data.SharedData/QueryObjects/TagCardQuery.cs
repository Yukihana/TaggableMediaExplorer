namespace TTX.Data.SharedData.QueryObjects;

public record TagCardRequest(string[] TagIds, bool AutoAdd = true);
public record TagCardResponse(TagCardState[] Results, string[] Failures);