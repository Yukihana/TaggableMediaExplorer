using TTX.Client.ViewLogic;

namespace TTX.Client.Extensions;

internal static class ModelHelper
{
    internal static AssetCardLogic CreateAssetLogic(this string id)
        => new() { ItemIdString = id };
}