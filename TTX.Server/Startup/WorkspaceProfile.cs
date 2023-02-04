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
    public string CachePath { get; set; } = "Cache";

    // Acquisition

    public string[] Whitelist { get; set; } = { "*.*" };
    public string[] Blacklist { get; set; } = Array.Empty<string>();
    public string[] FinalAdds { get; set; } = Array.Empty<string>();

    // Integrity

    public TimeSpan HashExpiry { get; set; } = new(30, 0, 0, 0);

    // Config

    public bool UpdateConfirmations { get; set; } = false;

    // IAssetInfo

    public int ReadBufferSize { get; set; } = 4 * 1024 * 1024; // 4 MiB
    public long SmallComputeMaximumSize { get; set; } = 100 * 1024 * 1024; // 100 MiB
    public int HashProcessingConcurrency { get; set; } = 4;
    public int HashIOConcurrency { get; set; } = 1;
    public int MetadataConcurrency { get; set; } = 4;
    public int CrumbsCount { get; set; } = 16;

    // Thumbnails

    public string ThumbsPath { get; set; } = "Thumbnails";
    public float ThumbnailTime { get; set; } = 0.2f;
    public string ThumbnailFormat { get; set; } = "PNG";

    // Runtime

    [JsonIgnore]
    public string ServerRoot { get; set; } = string.Empty;

    // Unused : IServiceOptions

    public void Initialize()
    { }
}