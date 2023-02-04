using TTX.Library.Helpers;

namespace TTX.Services;

public static class ServiceHelper
{
    internal static T InitializeServiceOptions<T>(this IOptionsSet options) where T : IServiceOptions, new()
    {
        var result = options.CopyByReflection<T>();
        result.Initialize();
        return result;
    }
}