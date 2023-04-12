using System;
using System.IO;
using System.Text.Json.Serialization;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public class AssetSynchronisationOptions : IAssetSynchronisationOptions
{
    public string ServerRoot { get; set; } = string.Empty;
    public string AssetsPath { get; set; } = "Assets";
    public TimeSpan AssetValidity { get; set; } = TimeSpan.FromDays(30);
    public TimeSpan AssetSyncAttemptBaseInterval { get; set; } = TimeSpan.FromSeconds(2);
    public int AssetFullSyncAttemptsOnReload { get; set; } = 2;
    public int AssetFullSyncAttemptsOnEvent { get; set; } = 100;

    // Derived

    [JsonIgnore]
    public string AssetsPathFull { get; set; } = string.Empty;

    public void Initialize()
    {
        AssetsPathFull = Path.Combine(ServerRoot, AssetsPath);
    }
}