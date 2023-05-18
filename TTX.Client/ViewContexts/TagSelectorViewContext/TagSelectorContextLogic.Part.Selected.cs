namespace TTX.Client.ViewContexts.TagSelectorViewContext;

public partial class TagSelectorContextLogic
{
    private string? _selectedTag = string.Empty;

    public string? SelectedTag
    {
        get { return _selectedTag; }
        set
        {
            _selectedTag = value;
            OnPropertyChanged(nameof(SelectedTag));
            SearchText = value ?? string.Empty;
        }
    }
}