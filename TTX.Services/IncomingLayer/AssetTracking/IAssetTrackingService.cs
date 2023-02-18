using System;
using System.Collections.Generic;
using System.Threading;

namespace TTX.Services.IncomingLayer.AssetTracking;

public interface IAssetTrackingService
{
    HashSet<string> GetAllFiles(CancellationToken ctoken = default);

    string[] Dequeue();

    // FSW

    void StartWatcher(Action onEnqueueAction);

    void StopWatcher();

    void ClearPending();
}