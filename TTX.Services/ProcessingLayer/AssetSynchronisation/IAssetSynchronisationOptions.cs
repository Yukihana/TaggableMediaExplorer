using System;

namespace TTX.Services.ProcessingLayer.AssetSynchronisation;

public interface IAssetSynchronisationOptions : IServiceProfile
{
    string AssetsPath { get; set; }
    TimeSpan AssetValidity { get; set; }
    TimeSpan AssetSyncAttemptBaseInterval { get; set; }
    int AssetFullSyncAttemptsOnReload { get; set; }
    int AssetFullSyncAttemptsOnEvent { get; set; }
}