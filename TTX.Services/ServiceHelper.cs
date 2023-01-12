using TTX.Library.Helpers;

namespace TTX.Services;

public static class ServiceHelper
{
    internal static T ExtractValues<T>(this object source) where T : IServiceOptions, new()
    {
        T options = source.CopyValues<T>().CopyFullyDecoupled();
        options.Initialize();
        return options;
    }
}