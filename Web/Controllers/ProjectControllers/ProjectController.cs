using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projects.Services;
using Web.Extensions.ProjectExtensions;
using Web.Requests.ProjectRequests;
using Web.Responses.ProjectResponses;

namespace Web.Controllers.ProjectControllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProject))]
    public IActionResult CreateProject([FromBody] CreateProjectApiRequest apiRequest)
    {
        var project = apiRequest.ToProject();
        var projectId = _projectService.CreateProject(project);
        var createdProject = _projectService.GetProject(projectId);
        return Ok(createdProject.ToApiProject());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProject))]
    public IActionResult GetProject(int id)
    {
        var project = _projectService.GetProject(id);
        return Ok(project.ToApiProject());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProject(int id, [FromBody] UpdateProjectApiRequest apiRequest)
    {
        var project = apiRequest.ToProject(id);
        _projectService.UpdateProject(project);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProject(int id)
    {
        _projectService.DeleteProject(id);
        return Ok();
    }

    [HttpGet("calculateTotalCost/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
    public IActionResult CalculateTotalCost(int projectId)
    {
        var totalCost = _projectService.CalculateTotalCost(projectId);
        return Ok(totalCost);
    }

    [HttpGet("calculateAverageProductivity/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
    public IActionResult CalculateAverageProductivity(int projectId)
    {
        var averageProductivity = _projectService.CalculateAverageProductivity(projectId);
        return Ok(averageProductivity);
    }

    [HttpPost("suspend/{projectId}")]
    public IActionResult SuspendProject(int projectId)
    {
        _projectService.SuspendProject(projectId);
        return Ok();
    }

    [HttpPost("continue/{projectId}")]
    public IActionResult ContinueProject(int projectId)
    {
        _projectService.ContinueProject(projectId);
        return Ok();
    }
}