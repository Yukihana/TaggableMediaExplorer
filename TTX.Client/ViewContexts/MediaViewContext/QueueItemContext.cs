using CommunityToolkit.Mvvm.ComponentModel;

namespace TTX.Client.ViewContexts.MediaViewContext;

public partial class QueueItemContext : ObservableObject
{
    // For designer purposes
    public QueueItemContext()
        => _card = new();

    // Actual initialization, called by code.
    public QueueItemContext(AssetCardContext assetCardContext)
        => _card = assetCardContext;

    // Contained object
    [ObservableProperty]
    private AssetCardContext _card;

    [ObservableProperty]
    private string _cachePath = string.Empty;
}