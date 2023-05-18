using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using TTX.Client.Services.ApiConnection;
using TTX.Client.Services.GuiSync;

namespace TTX.Client.ViewContexts.TagSelectorViewContext;

public partial class TagSelectorContextLogic : ObservableObject
{
    // Services

    private readonly IGuiSyncService _guiSync;
    private readonly IApiConnectionService _apiConnection;

    // Addons

    public ILogger<TagSelectorContextLogic>? Logger = null;

    // Properties

    [ObservableProperty]
    private string _title = "Specify a tag...";

    [ObservableProperty]
    private ObservableCollection<string> _matchingTags = new();

    // Init

    public TagSelectorContextLogic() : base()
    {
        _guiSync = ClientContextHost.GetService<IGuiSyncService>();
        _apiConnection = ClientContextHost.GetService<IApiConnectionService>();
        Logger = ClientContextHost.GetService<ILogger<TagSelectorContextLogic>>();
    }
}