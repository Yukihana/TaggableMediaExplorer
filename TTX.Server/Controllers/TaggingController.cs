using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;
using TTX.Services.ApiLayer.AssetTagging;

namespace TTX.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaggingController : ControllerBase
{
    private readonly IAssetTaggingService _assetTagging;
    private readonly ILogger<TaggingController> _logger;

    public TaggingController(
        IAssetTaggingService assetTagging,
        ILogger<TaggingController> logger
        ) : base()
    {
        _assetTagging = assetTagging;
        _logger = logger;
    }

    // POST: api/Tagging
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TaggingRequest request, CancellationToken ctoken = default)
    {
        try
        {
            return Ok(await _assetTagging.Apply(request, ctoken).ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            _logger.LogRequestError(ex, request);
            return BadRequest(ex);
        }
    }
}