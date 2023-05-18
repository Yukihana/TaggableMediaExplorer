using CommunityToolkit.Mvvm.ComponentModel;

namespace TTX.Client.ViewContexts;

public partial class TagCardContext : ObservableObject
{
    [ObservableProperty]
    public string _tagId = string.Empty;

    [ObservableProperty]
    public string _title = string.Empty;

    [ObservableProperty]
    public string _description = string.Empty;

    [ObservableProperty]
    public string _color = string.Empty;
}