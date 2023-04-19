using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using TTX.Client.ViewContexts.BrowserViewContext;
using TTX.Client.ViewContexts.MediaViewContext;

namespace TTX.Client.ViewContexts.MainViewContext;

public partial class MainContextLogic : ObservableObject
{
    // Addons

    public ILogger<MainContextLogic>? Logger { get; init; } = null;

    // Data and SubContexts

    [ObservableProperty]
    private MainContextData _contextData = new();

    [ObservableProperty]
    private BrowserContextLogic _browserContext = new();

    [ObservableProperty]
    private MediaContextLogic _mediaContext = new();

    // Commands

    public RelayCommand PlayItemsCommand { get; set; }

    // Initialize

    public MainContextLogic() : base()
    {
        PlayItemsCommand = new(PlayItems);
    }

    // Gui Events

    public void GuiLoaded()
        => BrowserContext.GuiLoaded();

    // Command actions

    public void PlayItems()
        => MediaContext.PlayItems(BrowserContext.ContextData.SelectedItems);
}