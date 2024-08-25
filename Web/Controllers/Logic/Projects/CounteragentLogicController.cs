using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projects.Services;

namespace Web.Controllers.Logic.Projects;

[ApiController]
[Route("api/counteragents/logic")]
public class CounteragentLogicController : ControllerBase
{
    private readonly ILogger<CounteragentLogicController> _logger;
    private readonly ICounteragentService _counteragentService;

    public CounteragentLogicController(ILogger<CounteragentLogicController> logger)
    {
        _logger = logger;
    }
}