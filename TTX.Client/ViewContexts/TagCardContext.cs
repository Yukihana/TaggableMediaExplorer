using CommunityToolkit.Mvvm.ComponentModel;

namespace TTX.Client.ViewContexts;

public partial class TagCardContext : ObservableObject
{
    [ObservableProperty]
    private string _tagId = string.Empty;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _color = "#888";
}