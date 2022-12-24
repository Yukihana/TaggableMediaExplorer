using System.Collections.Generic;

namespace TTX.Data.Messages;

public class TableData<T> : IMessage
{
    public string TargetSID { get; set; } = string.Empty;
    public List<T> Entities { get; set; } = new();
}