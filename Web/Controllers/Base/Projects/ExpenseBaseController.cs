using Contracts.ProjectEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Projects.Services;
using Web.Extensions.ProjectExtensions;
using Web.Requests.ProjectRequests;

namespace Web.Controllers.Base.Projects;

[ApiController]
[Route("api/expenses/base")]
public class ExpenseBaseController : ControllerBase
{
    private readonly ILogger<ExpenseBaseController> _logger;
    private readonly IExpenseService _expenseService;

    public ExpenseBaseController(ILogger<ExpenseBaseController> logger, IExpenseService expenseService)
    {
        _logger = logger;
        _expenseService = expenseService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Expense))]
    public async Task<IActionResult> AddExpense([FromBody] CreateExpenseApiRequest request, CancellationToken ct)
    {
        try
        {
            var addExpenseRequest = request.ToCreateExpenseApiRequest();
            var createdExpense = await _expenseService.CreateExpenseAsync(addExpenseRequest, ct);
            
            _logger.LogInformation("Расход {@ExpenseName} успешно добавлен на проект {@ProjectName}", 
                createdExpense.Name, createdExpense.Project.Name);

            return Ok(createdExpense);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении расхода");
            return BadRequest($"Ошибка при добавлении расхода {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Expense))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedExpense = request.ToUpdateExpenseApiRequest();

            await _expenseService.UpdateExpenseAsync(updatedExpense, ct);

            _logger.LogInformation("Расход {@Name} успешно обновлен на проекте {@ProjectName}", 
                updatedExpense.Name, updatedExpense.Project);
            
            return Ok(updatedExpense);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о расходе");
            return BadRequest($"Ошибка при обновлении информации о расходе {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Expense))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExpense(int id, CancellationToken ct)
    {
        try
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id, ct);
            if (expense == null)
            {
                _logger.LogWarning("Расход с ID {Id} не найден", id);
                return NotFound($"Расход с ID {id} не найден");
            }

            return Ok(expense);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о расходе");
            return BadRequest($"Ошибка при получении данных о расходе: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteExpense(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _expenseService.DeleteExpenseAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Расход с ID {Id} не найден", id);
                return NotFound($"Расход с ID {id} не найден");
            }

            _logger.LogInformation("Расход с ID {Id} успешно удален", id);
            return Ok();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23503")
        {
            _logger.LogError(ex, "Ошибка при удалении расхода из-за внешних ключей");
            return BadRequest("Невозможно удалить расход, так как он связан с другими записями.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении расхода");
            return BadRequest($"Ошибка при удалении расхода: {ex.Message}");
        }
    }
    
    [HttpGet("byProject/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Expense>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExpensesByProjectId(int projectId, CancellationToken ct)
    {
        try
        {
            var expenses = 
                await _expenseService.GetExpensesByProjectIdAsync(projectId, ct);
            if (!expenses.Any())
            {
                _logger.LogWarning("Проекта с ID {ProjectId} не найдено или нет статей расхода", projectId);
                return NotFound($"Проекта с ID {projectId} не найдено или нет статей расхода");
            }

            return Ok(expenses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении статей расхода по ID проекта");
            return BadRequest($"Ошибка при получении статей расхода по ID проекта: {ex.Message}");
        }
    }
}