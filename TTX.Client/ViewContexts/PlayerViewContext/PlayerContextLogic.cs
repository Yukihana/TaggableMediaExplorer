using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.GuiSync;

namespace TTX.Client.ViewContexts.PlayerViewContext;

public partial class PlayerContextLogic : ObservableObject
{
    private readonly IGuiSyncService _guiSync;

    public PlayerContextLogic()
    {
        _guiSync = ClientContextHost.GetService<IGuiSyncService>();
    }

    [ObservableProperty]
    private string _activeMediaPath = string.Empty;

    private TaskCompletionSource? _mediaEndedTaskCompletionSource = null;

    public Task PlayAsync(string path, CancellationToken ctoken = default)
    {
        // Cancel previous async
        _mediaEndedTaskCompletionSource?.SetCanceled(ctoken);
        ctoken.ThrowIfCancellationRequested();

        // Register media ended callback
        TaskCompletionSource tcs = new();

        // Start playback
        _guiSync.DispatchPost(() => ActiveMediaPath = path, ctoken);

        // Keep a reference and return
        _mediaEndedTaskCompletionSource = tcs;
        return tcs.Task;
    }

    public void OnPlaybackEnded()
        => _mediaEndedTaskCompletionSource?.TrySetResult();
}