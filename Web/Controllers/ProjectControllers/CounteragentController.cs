using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projects.Services;
using Web.Extensions.ProjectExtensions;
using Web.Requests.ProjectRequests;
using Web.Responses.ProjectResponses;

namespace Web.Controllers.ProjectControllers;

[ApiController]
[Route("api/counteragents")]
public class CounteragentController : ControllerBase
{
    private readonly ICounteragentService _counteragentService;

    public CounteragentController(ICounteragentService counteragentService)
    {
        _counteragentService = counteragentService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiCounteragent))]
    public IActionResult CreateCounteragent([FromBody] CreateCounteragentApiRequest apiRequest)
    {
        var counteragent = apiRequest.ToCounteragent();
        var counteragentId = _counteragentService.CreateCounteragent(counteragent);
        var createdCounteragent = _counteragentService.GetCounteragent(counteragentId);
        return Ok(createdCounteragent.ToApiCounteragent());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiCounteragent))]
    public IActionResult GetCounteragent(int id)
    {
        var counteragent = _counteragentService.GetCounteragent(id);
        return Ok(counteragent.ToApiCounteragent());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCounteragent(int id, [FromBody] UpdateCounteragentApiRequest apiRequest)
    {
        var counteragent = apiRequest.ToCounteragent(id);
        _counteragentService.UpdateCounteragent(counteragent);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCounteragent(int id)
    {
        _counteragentService.DeleteCounteragent(id);
        return Ok();
    }
}