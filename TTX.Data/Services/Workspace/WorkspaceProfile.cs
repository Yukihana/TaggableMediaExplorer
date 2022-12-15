using System;

namespace TTX.Data.Services.Workspace;

internal class WorkspaceProfile
{
    // Shared

    public string AssetsPath { get; set; } = string.Empty;
    public string ThumbsPath { get; set; } = string.Empty;
    public string CachePath { get; set; } = string.Empty;

    // Acquisition

    public string[] Whitelist { get; set; } = Array.Empty<string>();
    public string[] Blacklist { get; set; } = Array.Empty<string>();
    public string[] FinalAdds { get; set; } = Array.Empty<string>();

    // Integrity

    public TimeSpan HashExpiry { get; set; } = new(30, 0, 0, 0);

    // Config

    public bool UpdateConfirmations { get; set; } = false;
}