using CommunityToolkit.Mvvm.ComponentModel;

namespace TTX.Client.ViewData;

public partial class TagButtonData : ObservableObject
{
    [ObservableProperty]
    private string _tagId = string.Empty;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _vectorIcon = string.Empty;

    [ObservableProperty]
    private string _color0 = string.Empty;

    [ObservableProperty]
    private string _color1 = string.Empty;
}