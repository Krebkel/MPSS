using Contracts.EmployeeEntities;
using Employees.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Web.Extensions.EmployeeExtensions;
using Web.Requests.EmployeeRequests;

namespace Web.Controllers.Base.Employees;

[ApiController]
[Route("api/employeeShifts/base")]
public class EmployeeShiftBaseController : ControllerBase
{
    private readonly ILogger<EmployeeShiftBaseController> _logger;
    private readonly IEmployeeShiftService _employeeShiftService;

    public EmployeeShiftBaseController(ILogger<EmployeeShiftBaseController> logger, IEmployeeShiftService employeeShiftService)
    {
        _logger = logger;
        _employeeShiftService = employeeShiftService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeShift))]
    public async Task<IActionResult> AddEmployeeShift(
        [FromBody] CreateEmployeeShiftApiRequest request, CancellationToken ct)
    {
        try
        {
            var addEmployeeShiftRequest = request.ToCreateEmployeeShiftApiRequest();
            var createdEmployeeShift = await _employeeShiftService.CreateEmployeeShiftAsync(addEmployeeShiftRequest, ct);
            
            _logger.LogInformation("Смена сотрудника {@EmployeeName} успешно добавлена: {@Date}", 
                createdEmployeeShift.Employee.Name, createdEmployeeShift.Date);

            return Ok(createdEmployeeShift);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении смены");
            return BadRequest($"Ошибка при добавлении смены {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeShift))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEmployeeShift(
        [FromBody] UpdateEmployeeShiftApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedEmployeeShift = request.ToUpdateEmployeeShiftApiRequest();

            await _employeeShiftService.UpdateEmployeeShiftAsync(updatedEmployeeShift, ct);

            _logger.LogInformation("Смена сотрудника {@EmployeeName} успешно обновлена: {@Date}", 
                updatedEmployeeShift.Employee, updatedEmployeeShift.Date);
            
            return Ok(updatedEmployeeShift);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о смене");
            return BadRequest($"Ошибка при обновлении информации о смене {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeShift))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployeeShift(int id, CancellationToken ct)
    {
        try
        {
            var employeeShift = await _employeeShiftService.GetEmployeeShiftByIdAsync(id, ct);
            if (employeeShift == null)
            {
                _logger.LogWarning("Смена с ID {Id} не найдена", id);
                return NotFound($"Смена с ID {id} не найдена");
            }

            return Ok(employeeShift);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о смене");
            return BadRequest($"Ошибка при получении данных о смене: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteEmployeeShift(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _employeeShiftService.DeleteEmployeeShiftAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Смена с ID {Id} не найдена", id);
                return NotFound($"Смена с ID {id} не найдена");
            }

            _logger.LogInformation("Смена с ID {Id} успешно удалена", id);
            return Ok();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23503")
        {
            _logger.LogError(ex, "Ошибка при удалении смены из-за внешних ключей");
            return BadRequest("Невозможно удалить смену, так как она связана с другими записями.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении смена");
            return BadRequest($"Ошибка при удалении смены: {ex.Message}");
        }
    }
    
    [HttpGet("byProject/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployeeShiftsByProjectId(int projectId, CancellationToken ct)
    {
        try
        {
            var employeeShifts = await _employeeShiftService
                .GetEmployeeShiftsByProjectIdAsync(projectId, ct);
            return Ok(employeeShifts);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении смен по ID проекта");
            return BadRequest($"Ошибка при получении смен по ID проекта: {ex.Message}");
        }
    }
    
    [HttpGet("byEmployee/{employeeId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployeeShiftsByEmployeeId(int employeeId, CancellationToken ct)
    {
        try
        {
            var employeeShifts = await _employeeShiftService
                .GetEmployeeShiftsByEmployeeIdAsync(employeeId, ct);
            return Ok(employeeShifts);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении смен по ID сотрудника");
            return BadRequest($"Ошибка при получении смен по ID сотрудника: {ex.Message}");
        }
    }
}