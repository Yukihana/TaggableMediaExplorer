using System;
using System.Text.Json.Serialization;
using TTX.Services;

namespace TTX.Server.Startup;

public class WorkspaceProfile : IOptionsSet
{
    // Database

    public string AssetsDbFilename { get; set; } = "Assets.sqlite";

    // Shared

    public string AssetsPath { get; set; } = "Assets";
    public string ThumbsPath { get; set; } = "Thumbnails";
    public string CachePath { get; set; } = "Cache";

    // Acquisition

    public string[] Whitelist { get; set; } = { "*.*" };
    public string[] Blacklist { get; set; } = Array.Empty<string>();
    public string[] FinalAdds { get; set; } = Array.Empty<string>();

    // Integrity

    public TimeSpan HashExpiry { get; set; } = new(30, 0, 0, 0);

    // Thumbnails

    public float ThumbnailTime { get; set; } = 0.2f;
    public string ThumbnailFormat { get; set; } = "PNG";

    // Config

    public bool UpdateConfirmations { get; set; } = false;

    // Runtime

    [JsonIgnore]
    public string ServerRoot { get; set; } = string.Empty;
}