using Microsoft.AspNetCore.Mvc;
using TTX.Services.ApiLayer.AssetCardData;

namespace TTX.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssetDataController : ControllerBase
{
    private readonly IAssetCardDataService _assetCardData;

    public AssetDataController(IAssetCardDataService assetCardData)
    {
        _assetCardData = assetCardData;
    }

    [HttpGet]
    [Route("Card")]
    public ActionResult Get([FromQuery] string id)
        => Ok(_assetCardData.GetAssetCardData(id));
}