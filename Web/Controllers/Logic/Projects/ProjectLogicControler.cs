using Employees.Services;
using Microsoft.AspNetCore.Http;
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
    private readonly IEmployeeShiftService _employeeShiftService;

    public ProjectLogicController(
        ILogger<ProjectLogicController> logger,
        IProjectService projectService,
        IEmployeeShiftService employeeShiftService)
    {
        _logger = logger;
        _projectService = projectService;
        _employeeShiftService = employeeShiftService;
    }

    [HttpGet("wages/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProjectWages(int id, CancellationToken ct)
    {
        try
        {
                var result = await _projectService.CalculateProjectWagesAsync(id, ct);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Проект не найден");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при расчете зарплат для проекта");
            return BadRequest($"Ошибка при расчете зарплат: {ex.Message}");
        }
    }
}