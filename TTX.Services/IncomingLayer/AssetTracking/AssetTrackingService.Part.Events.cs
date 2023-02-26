using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TTX.Library.FileSystemHelpers;

namespace TTX.Services.IncomingLayer.AssetTracking;

public partial class AssetTrackingService
{
    public partial void OnError(object sender, ErrorEventArgs e)
        => _logger.LogError(e.GetException(), "The tracker has encountered an error.", e);

    private partial void EnqueueValidated(params string[] paths)
    {
        if (_watcherAction is Action<string, DateTime> action)
        {
            foreach (string path in paths)
            {
                if (ValidatePathByPattern(path))
                    action(path, DateTime.UtcNow);
            }
        }
    }

    private partial bool ValidatePathByPattern(string path)
    {
        // see if the algo can be improved based on this library class
        // https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-7.0

        ReadOnlySpan<char> pathSpan = path.AsSpan();

        foreach (string pattern in _options.FinalAdds)
        {
            if (pathSpan.MatchByPattern(pattern.AsSpan()))
                return true;
        }

        foreach (string pattern in _options.Blacklist)
        {
            if (pathSpan.MatchByPattern(pattern.AsSpan()))
                return false;
        }

        foreach (string pattern in _options.Whitelist)
        {
            if (pathSpan.MatchByPattern(pattern.AsSpan()))
                return true;
        }

        return false;
    }
}