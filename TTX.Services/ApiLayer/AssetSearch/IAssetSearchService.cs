using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Services.ApiLayer.AssetSearch;

public interface IAssetSearchService
{
    Task<SearchResponse> Search(SearchRequest query, CancellationToken ctoken = default);
}