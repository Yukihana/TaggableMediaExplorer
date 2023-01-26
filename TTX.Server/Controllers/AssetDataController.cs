using Microsoft.AspNetCore.Mvc;
using TTX.Services.QueryApi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTX.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssetDataController : ControllerBase
{
    private readonly IQueryApiService _queryApi;

    public AssetDataController(IQueryApiService queryApi)
    {
        _queryApi = queryApi;
    }

    [HttpGet]
    [Route("Card")]
    public ActionResult Get([FromQuery] string guid)
        => Ok(_queryApi.GetAssetCard(guid));

    /*
    // GET: api/<AssetDataController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<AssetDataController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<AssetDataController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<AssetDataController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<AssetDataController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
    */
}