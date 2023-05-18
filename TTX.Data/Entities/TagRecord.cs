using System.ComponentModel.DataAnnotations;

namespace TTX.Data.Entities;

public class TagRecord
{
    [Key]
    public int ID { get; set; } = 0;

    public string TagId { get; set; } = "unique-tag-id-0";

    // Tag

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    // Server data

    public bool IsEnabled { get; set; } = true;
}