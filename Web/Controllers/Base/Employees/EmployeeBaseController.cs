using Contracts.EmployeeEntities;
using Employees.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Extensions.EmployeeExtensions;
using Web.Requests.EmployeeRequests;

namespace Web.Controllers.Base.Employees;

[ApiController]
[Route("api/employees/base")]
public class EmployeeBaseController : ControllerBase
{
    private readonly ILogger<EmployeeBaseController> _logger;
    private readonly IEmployeeService _employeeService;

    public EmployeeBaseController(ILogger<EmployeeBaseController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
    public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeApiRequest request, CancellationToken ct)
    {
        try
        {
            var addEmployeeRequest = request.ToCreateEmployeeRequest();
            var createdEmployee = await _employeeService.CreateEmployeeAsync(addEmployeeRequest, ct);
            
            _logger.LogInformation("Сотрудник успешно добавлен: {@Name}", createdEmployee.Name);

            return Ok(createdEmployee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении сотрудника");
            return BadRequest($"Ошибка при добавлении сотрудника {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(request.ToUpdateEmployeeRequest(), ct);
            if (updatedEmployee == null)
            {
                _logger.LogWarning("Сотрудник с ID {Id} не найден", request.Id);
                return NotFound($"Сотрудник с ID {request.Id} не найден");
            }

            _logger.LogInformation("Сотрудник успешно обновлен: {@Name}", updatedEmployee.Name);
            return Ok(updatedEmployee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о сотруднике");
            return BadRequest($"Ошибка при обновлении информации о сотруднике {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployee(int id, CancellationToken ct)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id, ct);
            if (employee == null)
            {
                _logger.LogWarning("Сотрудник с ID {Id} не найден", id);
                return NotFound($"Сотрудник с ID {id} не найден");
            }

            return Ok(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о сотруднике");
            return BadRequest($"Ошибка при получении данных о сотруднике: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployee(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _employeeService.DeleteEmployeeAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Сотрудник с ID {Id} не найден", id);
                return NotFound($"Сотрудник с ID {id} не найден");
            }

            _logger.LogInformation("Сотрудник с ID {Id} успешно удален", id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении сотрудника");
            return BadRequest($"Ошибка при удалении сотрудника: {ex.Message}");
        }
    }
}