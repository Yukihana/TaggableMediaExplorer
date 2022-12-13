namespace TTX.Data.Services.Acquisition
{
    public interface IWorkspaceService
    {
        string AssetsPathFull { get; }

        string? LocalizePath(string path);
    }
}