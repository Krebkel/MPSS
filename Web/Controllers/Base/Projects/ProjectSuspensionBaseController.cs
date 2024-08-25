using Contracts.ProjectEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projects.Services;
using Web.Extensions.ProjectExtensions;
using Web.Requests.ProjectRequests;

namespace Web.Controllers.Base.Projects;

[ApiController]
[Route("api/projectSuspension/base")]
public class ProjectSuspensionBaseController : ControllerBase
{
    private readonly ILogger<ProjectSuspensionBaseController> _logger;
    private readonly IProjectSuspensionService _projectSuspensionService;

    public ProjectSuspensionBaseController(ILogger<ProjectSuspensionBaseController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectSuspension))]
    public async Task<IActionResult> AddProjectSuspension(
        [FromBody] CreateProjectSuspensionApiRequest request, CancellationToken ct)
    {
        try
        {
            var addProjectSuspensionRequest = request.ToCreateProjectSuspensionApiRequest();
            var createdProjectSuspension = await _projectSuspensionService
                .CreateProjectSuspensionAsync(addProjectSuspensionRequest, ct);
            
            _logger.LogInformation("Информация о приостановке проекта {@ProjectName} успешно добавлена", 
                createdProjectSuspension.Project.Name);

            return Ok(createdProjectSuspension);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении информации о приостановке проекта");
            return BadRequest($"Ошибка при добавлении информации о приостановке проекта {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectSuspension))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProjectSuspension(
        [FromBody] UpdateProjectSuspensionApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedProjectSuspension = request.ToUpdateProjectSuspensionApiRequest();

            await _projectSuspensionService.UpdateProjectSuspensionAsync(updatedProjectSuspension, ct);

            _logger.LogInformation("Информация о приостановке проекта {@ProjectName} успешно обновлена", 
                request.Project.Name);
            
            return Ok(updatedProjectSuspension);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о приостановке проекта");
            return BadRequest($"Ошибка при обновлении информации о приостановке проекта {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectSuspension))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectSuspension(int id, CancellationToken ct)
    {
        try
        {
            var projectSuspension = await _projectSuspensionService.GetProjectSuspensionByIdAsync(id, ct);
            if (projectSuspension == null)
            {
                _logger.LogWarning("Приостановка проекта с ID {Id} не найдена", id);
                return NotFound($"Приостановка проекта с ID {id} не найдена");
            }

            return Ok(projectSuspension);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о приостановке проекта");
            return BadRequest($"Ошибка при получении данных о приостановке проекта: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProjectSuspension(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _projectSuspensionService.DeleteProjectSuspensionAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Приостановка проекта с ID {Id} не найдена", id);
                return NotFound($"Приостановка проекта с ID {id} не найдена");
            }

            _logger.LogInformation("Приостановка проекта с ID {Id} успешно удалена", id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении приостановки проекта");
            return BadRequest($"Ошибка при удалении приостановки проекта: {ex.Message}");
        }
    }
}