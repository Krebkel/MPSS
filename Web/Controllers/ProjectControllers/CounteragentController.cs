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
        try
        {
            var counteragent = apiRequest.ToCounteragent();
            var counteragentId = _counteragentService.CreateCounteragent(counteragent);
            var createdCounteragent = _counteragentService.GetCounteragent(counteragentId);
            return Ok(createdCounteragent.ToApiCounteragent());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при создании контрагента");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiCounteragent))]
    public IActionResult GetCounteragent(int id)
    {
        var counteragent = _counteragentService.GetCounteragent(id);
        return Ok(counteragent.ToApiCounteragent());
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApiCounteragent>))]
    public IActionResult GetAllCounteragents()
    {
        var counteragents = _counteragentService.GetAllCounteragents();
        
        var apiCounteragents = counteragents.Select(c => new ApiCounteragent()
        {
            Id = c.Id,
            Name = c.Name,
            AccountNumber = c.AccountNumber,
            BIK = c.BIK,
            Contact = c.Contact,
            Phone = c.Phone,
            INN = c.INN,
            OGRN = c.OGRN
        }).ToList();        
        
        return Ok(apiCounteragents);
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