using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.Legacy.AssetsIndexer;

public class AssetsIndexerOptions : IAssetsIndexerOptions
{
    public string ServerRoot { get; set; } = string.Empty;
    public string AssetsPath { get; set; } = "Assets";

    // Derived

    [JsonIgnore]
    public string AssetsPathFull { get; set; } = string.Empty;

    public void Initialize()
    {
        AssetsPathFull = Path.Combine(ServerRoot, AssetsPath);
    }
}