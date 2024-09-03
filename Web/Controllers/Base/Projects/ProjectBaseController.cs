using Contracts.ProjectEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projects.Services;
using Web.Extensions.ProjectExtensions;
using Web.Requests.ProjectRequests;

namespace Web.Controllers.Base.Projects;

[ApiController]
[Route("api/projects/base")]
[Authorize]
public class ProjectBaseController : ControllerBase
{
    private readonly ILogger<ProjectBaseController> _logger;
    private readonly IProjectService _projectService;

    public ProjectBaseController(ILogger<ProjectBaseController> logger, IProjectService projectService)
    {
        _logger = logger;
        _projectService = projectService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> AddProject([FromBody] CreateProjectApiRequest request, CancellationToken ct)
    {
        try
        {
            var addProjectRequest = request.ToCreateProjectApiRequest();
            var createdProject = await _projectService.CreateProjectAsync(addProjectRequest, ct);
            
            _logger.LogInformation("Проект {@ProjectName} успешно добавлен", request.Name);

            return Ok(createdProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении проекта");
            return BadRequest($"Ошибка при добавлении проекта {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedProject = request.ToUpdateProjectApiRequest();

            await _projectService.UpdateProjectAsync(updatedProject, ct);

            _logger.LogInformation("Проект {@ProjectName} успешно обновлен", request.Name);
            
            return Ok(updatedProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о проекте");
            return BadRequest($"Ошибка при обновлении информации о проекте {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> GetProject(int id, CancellationToken ct)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(id, ct);
            if (project == null)
            {
                _logger.LogWarning("Проект с ID {Id} не найден", id);
                return NotFound($"Проект с ID {id} не найден");
            }

            return Ok(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о проекте");
            return BadRequest($"Ошибка при получении данных о проекте: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> DeleteProject(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _projectService.DeleteProjectAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Проект с ID {Id} не найден", id);
                return NotFound($"Проект с ID {id} не найден");
            }

            _logger.LogInformation("Проект с ID {Id} успешно удален", id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении проекта");
            return BadRequest($"Ошибка при удалении проекта: {ex.Message}");
        }
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Project>))]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> GetAllProjects(CancellationToken ct)
    {
        try
        {
            var projects = await _projectService.GetAllProjectsAsync(ct);
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка проектов");
            return BadRequest($"Ошибка при получении списка проектов: {ex.Message}");
        }
    }
}