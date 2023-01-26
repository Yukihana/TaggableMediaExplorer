using Microsoft.AspNetCore.Mvc;
using TTX.Data.Shared.QueryObjects;
using TTX.Services.QueryApi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTX.Server.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IQueryApiService _queryApi;

    public SearchController(IQueryApiService queryApi)
    {
        _queryApi = queryApi;
    }

    [HttpGet]
    public ActionResult Get([FromQuery] SearchQuery query)
        => Ok(_queryApi.Search(query));

    /*
    // POST api/<SearchController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<SearchController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<SearchController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
    */
}