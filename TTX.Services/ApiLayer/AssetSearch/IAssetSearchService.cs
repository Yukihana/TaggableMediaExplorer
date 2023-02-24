using TTX.Data.Shared.QueryObjects;

namespace TTX.Services.ApiLayer.AssetSearch;

public interface IAssetSearchService
{
    SearchResponse Search(SearchQuery query);
}