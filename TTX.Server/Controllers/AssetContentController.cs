using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TTX.Services.ApiLayer.AssetContent;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTX.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetContentController : ControllerBase
    {
        private readonly IAssetContentService _assetContent;
        private readonly ILogger<AssetContentController> _logger;

        public AssetContentController(
            IAssetContentService assetContent,
            ILogger<AssetContentController> logger)
        {
            _assetContent = assetContent;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string id)
        {
            if (_assetContent.GetPath(id) is string path)
                return new PhysicalFileResult(path, "application/octet-stream");

            _logger.LogWarning("Bad request: {id}", id);
            return BadRequest();
        }
    }
}