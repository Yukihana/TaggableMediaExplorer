using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TTX.Services.ApiLayer.AssetSnapshotData;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTX.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetSnapshotController : ControllerBase
    {
        private readonly IAssetSnapshotDataService _assetSnapshotData;
        private readonly ILogger<AssetSnapshotController> _logger;

        public AssetSnapshotController(
            IAssetSnapshotDataService assetSnapshotData,
            ILogger<AssetSnapshotController> logger)
        {
            _assetSnapshotData = assetSnapshotData;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string id)
        {
            if (_assetSnapshotData.GetPath(id) is string path)
                return new PhysicalFileResult(path, "application/octet-stream");
            else
                return BadRequest();
        }
    }
}