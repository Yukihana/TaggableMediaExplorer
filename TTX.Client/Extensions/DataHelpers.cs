using TTX.Client.ViewLogic;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.Extensions;

internal static class DataHelpers
{
    internal static AssetLogic CreateAssetLogic(this AssetCard card)
    {
        AssetLogic result = new();
        result.LoadDataFrom(card);
        return result;
    }
}