using TTX.Services;

namespace TTX.Server.Startup;

public class RuntimeConfig : IRuntimeConfig
{
    public string ServerPath { get; init; } = string.Empty;
    public string ProfileRoot { get; init; } = string.Empty;
    public string ProfileFilename { get; init; } = string.Empty;
}