using TTX.Client.ViewContexts.MainViewContext;

namespace TTX.Client.Services.MainGui;

public interface IMainView
{
    void SetViewContext(MainContextLogic viewContext);

    void ShowView();
}