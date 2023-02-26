using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TTX.Services.ApiLayer.AssetCardData;

namespace TTX.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssetDataController : ControllerBase
{
    private readonly IAssetCardDataService _assetCardData;
    private readonly ILogger<AssetDataController> _logger;

    public AssetDataController(IAssetCardDataService assetCardData, ILogger<AssetDataController> logger)
    {
        _assetCardData = assetCardData;
        _logger = logger;
    }

    [HttpGet]
    [Route("Card")]
    public async Task<IActionResult> Get([FromQuery] string id)
    {
        try
        {
            return Ok(await _assetCardData.GetAssetCardData(id).ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Encountered an error with the request: {request}", id);
            return BadRequest();
        }
    }
}