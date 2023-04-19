using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using TTX.Client.Services.AssetLoader;
using TTX.Client.Services.GuiSync;

namespace TTX.Client.ViewContexts.MediaViewContext;

public partial class MediaContextLogic : ObservableObject
{
    private readonly IGuiSyncService _guiSync;
    private readonly IAssetLoaderService _assetLoader;

    [ObservableProperty]
    private MediaContextData _contextData = new();

    [ObservableProperty]
    private RelayCommand _playSelectedCommand;

    public MediaContextLogic()
    {
        _guiSync = ClientContextHost.GetService<IGuiSyncService>();
        _assetLoader = ClientContextHost.GetService<IAssetLoaderService>();

        _playSelectedCommand = new(PlaySelected);
        ContextData.PropertyChanged += ContextDataPropertyChanged;
    }

    private void ContextDataPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MediaContextData.ControlEventsPipe))
            HandlePlayerEvent(ContextData.ControlEventsPipe);
    }
}