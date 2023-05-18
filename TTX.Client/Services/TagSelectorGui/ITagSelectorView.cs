using TTX.Client.ViewContexts.TagSelectorViewContext;

namespace TTX.Client.Services.TagSelectorGui;

public interface ITagSelectorView
{
    void SetViewContext(TagSelectorContextLogic viewContext);

    string? ShowModal();
}