namespace TTX.Client.Services.MainGui;

public interface IMainView
{
    void SetViewContext(MainLogic mainLogic);

    void ShowView();
}