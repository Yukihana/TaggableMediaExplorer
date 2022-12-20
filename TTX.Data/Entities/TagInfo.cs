namespace TTX.Data.Entities;

public class TagInfo
{
    public int ID { get; set; } = 0;
    public string Name { get; set; } = string.Empty;

    // Tag

    public string Description { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public bool AllowAssign { get; set; } = true;
}