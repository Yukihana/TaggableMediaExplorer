using System.IO;
using System.Text.Json;
using TTX.Data.Services.Acquisition;

namespace TTX.Data.Services.Workspace;

/// <summary>
/// Service for coordinate via workspace parameters.
/// </summary>
public class WorkspaceService : IWorkspaceService
{
    private WorkspaceProfile _profile = new();
    private readonly WorkspaceOptions _options;

    public WorkspaceService(WorkspaceOptions options)
    {
        _options = options;
    }

    public bool Load()
    {
        var profile = Path.Combine(_options.Path, _options.Profile);
        try
        {
            if (File.Exists(profile))
            {
                _profile = JsonSerializer.Deserialize<WorkspaceProfile>(File.ReadAllText(profile)) ?? throw new InvalidDataException("Unable to deserialize profile.");
                return true;
            }
            File.WriteAllText(profile, JsonSerializer.Serialize(_profile));
        }
        catch { }
        return false;
    }

    public string AssetsPathFull => Path.Combine(_options.Path, _profile.AssetsPath);

    public string? LocalizePath(string path)
    {
        try
        {
            return Path.GetRelativePath(_options.Path, path);
        }
        catch
        {
            return path;
        }
    }
}