using Microsoft.AspNetCore.Mvc;
using TTX.Data.Shared.QueryObjects;
using TTX.Services.ApiLayer.AssetSearch;

namespace TTX.Server.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IAssetSearchService _assetSearch;

    public SearchController(IAssetSearchService assetSearch)
    {
        _assetSearch = assetSearch;
    }

    [HttpGet]
    public ActionResult Get([FromQuery] SearchQuery query)
        => Ok(_assetSearch.Search(query));
}