using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.ViewContexts.MediaViewContext;

public partial class MediaContextLogic
{
    // From queue

    private void PlaySelected()
    {
        if (ContextData.SelectedItems.FirstOrDefault(x => true, null) is QueueItemContext item)
            StartPlayback(item);
    }

    // From BrowserView

    private void PlayNext()
    {
        if (GetNext(ContextData.ActiveMedia) is QueueItemContext next)
            StartPlayback(next);
    }

    private QueueItemContext? GetNext(QueueItemContext? current)
    {
        // Snapshot items
        QueueItemContext[] items = ContextData.Items.ToArray();

        // If there's nothing, return null.
        if (!items.Any())
            return null;

        // Get current index
        int index = current is null ? -1 : Array.IndexOf(items, current);

        // if item doesn't exist, send first item in the queue
        if (index < 0)
            return items.First();

        // send next if within range
        if (index + 1 < items.Length)
            return items[index + 1];

        // if end reached, send null
        return null;
    }

    private void StartPlayback(QueueItemContext item)
    {
        string id = item.Card.ItemId;
        DateTime updatedUtc = item.Card.UpdatedUtc;
        _ = Task.Run(async () => await StartPlaybackAsync(id, updatedUtc, item, _guiSync.CancellationToken).ConfigureAwait(false));
    }

    private async Task StartPlaybackAsync(string idString, DateTime updatedUtc, QueueItemContext item, CancellationToken ctoken = default)
    {
        // Load resource and get path
        string cachepath = await _assetLoader.GetCachedPath(idString, updatedUtc, ctoken).ConfigureAwait(false);

        // Deploy
        _guiSync.DispatchPost(() =>
        {
            item.CachePath = cachepath;
            ContextData.ActiveMedia = item;
        }, ctoken);
    }

    // To be run on main thread

    private void StopPlayback()
        => ContextData.ControlCommandsPipe = "stop";
}