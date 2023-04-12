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

    // Database

    public bool EnableAssetDeletion { get; set; } = false;

    // Sync

    public TimeSpan AssetValidity { get; set; } = TimeSpan.FromDays(30);
    public TimeSpan AssetSyncAttemptBaseInterval { get; set; } = TimeSpan.FromSeconds(2);
    public int AssetFullSyncAttemptsOnReload { get; set; } = 2;
    public int AssetFullSyncAttemptsOnEvent { get; set; } = 100;

    // Config

    public bool UpdateConfirmations { get; set; } = false;

    // IAssetInfo

    public int ReadBufferSize { get; set; } = 4 * 1024 * 1024; // 4 MiB
    public long SmallComputeMaximumSize { get; set; } = 100 * 1024 * 1024; // 100 MiB
    public int HashProcessingConcurrency { get; set; } = 4;
    public int HashIOConcurrency { get; set; } = 1;
    public int MetadataConcurrency { get; set; } = 4;
    public int CrumbsCount { get; set; } = 16;

    // Previews

    public string PreviewsPath { get; set; } = "Previews";
    public float AssetPreviewSnapshotTime { get; set; } = 0.2f;

    // Runtime

    [JsonIgnore]
    public string ServerRoot { get; set; } = string.Empty;

    // Unused : IServiceOptions

    public void Initialize()
    { }
}