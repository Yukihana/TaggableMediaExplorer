using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Services.ApiLayer.TagData;

namespace TTX.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatedTagsController : ControllerBase
{
    private readonly ITagDataService _tagData;
    private readonly ILogger<RelatedTagsController> _logger;

    public RelatedTagsController(
        ITagDataService tagData,
        ILogger<RelatedTagsController> logger)
    {
        _tagData = tagData;
        _logger = logger;
    }

    // GET: api/RelatedTags
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string searchtext, CancellationToken ctoken = default)
    {
        try
        {
            return Ok(await _tagData.GetRelatedTags(searchtext, ctoken).ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            _logger.LogRequestError(ex, searchtext);
            return BadRequest();
        }
    }
}