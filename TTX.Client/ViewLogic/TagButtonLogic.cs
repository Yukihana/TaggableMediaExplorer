using CommunityToolkit.Mvvm.ComponentModel;
using TTX.Client.ViewData;

namespace TTX.Client.ViewLogic;

public partial class TagButtonLogic : ObservableObject
{
    [ObservableProperty]
    private TagButtonData _dataModel = new();
}