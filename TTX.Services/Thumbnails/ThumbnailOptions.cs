using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.Thumbnails;

internal class ThumbnailOptions : IThumbnailOptions
{
    public string ServerRoot { get; set; } = string.Empty;
    public string ThumbsPath { get; set; } = "Thumbnails";
    public float ThumbnailTime { get; set; } = 0.2f;
    public string ThumbnailFormat { get; set; } = "PNG";

    // Derived

    [JsonIgnore]
    public string ThumbsPathFull { get; set; } = string.Empty;

    public void Initialize()
    {
        ThumbsPathFull = Path.Combine(ServerRoot, ThumbsPath);
    }
}