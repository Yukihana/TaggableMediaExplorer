using TTX.Library.Helpers;

namespace TTX.Services;

public static class ServiceHelper
{
    internal static T InitializeServiceOptions<T>(
        this IWorkspaceProfile profile)
        where T : IServiceProfile, new()
    {
        var result = profile.CopyByReflection<T>();
        result.Initialize();
        return result;
    }

    internal static T InitializeServiceOptions<T>(
        this IWorkspaceProfile profile,
        IRuntimeConfig config)
        where T : IServiceProfile, new()
    {
        var result = profile.CopyByReflection<T>();
        result.Initialize(config);
        return result;
    }
}