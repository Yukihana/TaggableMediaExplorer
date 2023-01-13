using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Threading;

namespace TTX.Data.Entities;

public class TagRecord
{
    public int ID { get; set; } = 0;
    public string Name { get; set; } = string.Empty;

    // Tag

    public string Description { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public bool AllowAssign { get; set; } = true;

    // Not Mapped

    [NotMapped]
    [JsonIgnore]
    public SemaphoreSlim Semaphore { get; set; } = new(1);
}