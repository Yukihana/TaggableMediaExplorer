using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.SharedData.QueryObjects;
using TTX.Services.ApiLayer.TagData;

namespace TTX.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagDataService _tagData;
    private readonly ILogger<TagsController> _logger;

    public TagsController(
        ITagDataService tagData,
        ILogger<TagsController> logger
        ) : base()
    {
        _tagData = tagData;
        _logger = logger;
    }

    // POST: api/tags
    // Request contains array. Can be large.
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TagCardRequest request, CancellationToken ctoken = default)
    {
        try
        {
            return Ok(await _tagData.GetCards(request, ctoken).ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            _logger.LogRequestError(ex, request);
            return BadRequest();
        }
    }
}