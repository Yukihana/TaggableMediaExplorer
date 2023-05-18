using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Services.ApiLayer.AssetTagging;

public interface IAssetTaggingService
{
    Task<TaggingResponse> Apply(TaggingRequest request, CancellationToken ctoken = default);
}