using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace TTX.Client.ViewContexts.MediaViewContext;

public partial class MediaContextLogic
{
    // By BrowserView

    public void PlayItems(AssetCardContext[] items)
    {
        // Clear queue and add these
        ContextData.Items.Clear();
        foreach (var item in items)
            ContextData.Items.Add(new QueueItemContext(item));

        // Start playing
        PlayNext();
    }

    public void QueueItems(AssetCardContext[] items)
    {
        foreach (var item in items)
            ContextData.Items.Add(new QueueItemContext(item));
    }

    // By events

    private void HandlePlayerEvent(string output)
    {
        switch (output)
        {
            case "ended":
                PlayNext();
                break;

            default:
                break;
        }
    }
}