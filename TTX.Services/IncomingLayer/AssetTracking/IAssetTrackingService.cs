using System;
using System.Threading;

namespace TTX.Services.IncomingLayer.AssetTracking;

public interface IAssetTrackingService
{
    string[] GetAllFiles(CancellationToken ctoken = default);

    void StartWatcher(Action<string, DateTime> onEnqueueAction);

    void StopWatcher();
}