using TTX.Client.ViewLogic;

namespace TTX.Client.Extensions;

internal static class ModelHelper
{
    internal static AssetLogic CreateAssetLogic(this string guid)
        => new() { GUID = guid };
}