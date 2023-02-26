using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;
using TTX.Services.ApiLayer.AssetSearch;

namespace TTX.Server.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IAssetSearchService _assetSearch;
    private readonly ILogger<SearchController> _logger;

    public SearchController(IAssetSearchService assetSearch, ILogger<SearchController> logger)
    {
        _assetSearch = assetSearch;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SearchQuery query)
    {
        try
        {
            return Ok(await _assetSearch.Search(query).ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Encountered an error with the request: {request}", query);
            return BadRequest();
        }
    }
}