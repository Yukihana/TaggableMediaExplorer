using TTX.Client.ViewContexts;

namespace TTX.Client.Services.ClientApiServices.AssetClientApi;

internal interface IAssetClientApiService
{
    AssetCardContext GetAssetCard(string id);
}