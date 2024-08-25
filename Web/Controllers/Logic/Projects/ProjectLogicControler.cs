using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projects.Services;

namespace Web.Controllers.Logic.Projects;

[ApiController]
[Route("api/projects/logic")]
public class ProjectLogicController : ControllerBase
{
    private readonly ILogger<ProjectLogicController> _logger;
    private readonly IProjectService _projectService;

    public ProjectLogicController(ILogger<ProjectLogicController> logger)
    {
        _logger = logger;
    }
}