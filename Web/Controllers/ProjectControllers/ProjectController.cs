using Contracts.ProjectEntities;
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApiProject>))]
    public IActionResult GetAllProjects()
    {
        var projects = _projectService.GetAllProjects();
        
        var apiProjects = projects.Select(p => new ApiProject()
        {
            Id = p.Id,
            Name = p.Name,
            Address = p.Address,
            CounteragentId = p.CounteragentId,
            DeadlineDate = p.DeadlineDate,
            ProjectStatus = p.ProjectStatus,
            DateSuspended = p.DateSuspended,
            StartDate = p.StartDate,
            ResponsibleEmployeeId = p.ResponsibleEmployeeId,
            TotalCost = p.TotalCost
        }).ToList();        
        
        return Ok(apiProjects);
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

    [HttpPut("{projectId}/status")]
    public IActionResult UpdateProjectStatus(int projectId, [FromBody] string status)
    {
        if (Enum.TryParse(typeof(ProjectStatus), status, true, out var result))
        {
            var projectStatus = (ProjectStatus)result;

            var project = _projectService.GetProject(projectId);
            if (project == null)
            {
                return NotFound("Проект не найден.");
            }

            project.ProjectStatus = projectStatus;
            _projectService.UpdateProject(project);

            return Ok("Статус проекта успешно обновлен.");
        }
        else
        {
            return BadRequest("Неверный статус проекта.");
        }
    }
}