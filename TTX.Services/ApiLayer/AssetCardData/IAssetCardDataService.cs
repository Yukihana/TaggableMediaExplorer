using TTX.Data.Shared.QueryObjects;

namespace TTX.Services.ApiLayer.AssetCardData;

public interface IAssetCardDataService
{
    AssetCardResponse? GetAssetCardData(string id);
}