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
[Route("api/counteragents/base")]
public class CounteragentBaseController : ControllerBase
{
    private readonly ILogger<CounteragentBaseController> _logger;
    private readonly ICounteragentService _counteragentService;

    public CounteragentBaseController(ILogger<CounteragentBaseController> logger, 
        ICounteragentService counteragentService)
    {
        _logger = logger;
        _counteragentService = counteragentService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Counteragent))]
    public async Task<IActionResult> AddCounteragent(
        [FromBody] CreateCounteragentApiRequest request, CancellationToken ct)
    {
        try
        {
            var addCounteragentRequest = request.ToCreateCounteragentApiRequest();
            var createdCounteragent = await _counteragentService.CreateCounteragentAsync(addCounteragentRequest, ct);
            
            _logger.LogInformation("Контрагент успешно добавлен: {@Name}", createdCounteragent.Name);

            return Ok(createdCounteragent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении контрагента");
            return BadRequest($"Ошибка при добавлении контрагента {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Counteragent))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCounteragent(
        [FromBody] UpdateCounteragentApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedCounteragent = request.ToUpdateCounteragentApiRequest();

            await _counteragentService.UpdateCounteragentAsync(updatedCounteragent, ct);

            _logger.LogInformation("Контрагент успешно обновлен: {@Name}", request.Name);
            
            return Ok(updatedCounteragent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о контрагенте");
            return BadRequest($"Ошибка при обновлении информации о контрагенте {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Counteragent))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCounteragent(int id, CancellationToken ct)
    {
        try
        {
            var counteragent = await _counteragentService.GetCounteragentByIdAsync(id, ct);
            if (counteragent == null)
            {
                _logger.LogWarning("Контрагент с ID {Id} не найден", id);
                return NotFound($"Контрагент с ID {id} не найден");
            }

            return Ok(counteragent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о контрагенте");
            return BadRequest($"Ошибка при получении данных о контрагенте: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCounteragent(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _counteragentService.DeleteCounteragentAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Контрагент с ID {Id} не найден", id);
                return NotFound($"Контрагент с ID {id} не найден");
            }

            _logger.LogInformation("Контрагент с ID {Id} успешно удален", id);
            return Ok();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23503")
        {
            _logger.LogError(ex, "Ошибка при удалении контрагента из-за внешних ключей");
            return BadRequest("Невозможно удалить контрагента, так как он связан с другими записями.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении контрагента");
            return BadRequest($"Ошибка при удалении контрагента: {ex.Message}");
        }
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Counteragent>))]
    public async Task<IActionResult> GetAllCounteragents(CancellationToken ct)
    {
        try
        {
            var counteragents = await _counteragentService.GetAllCounteragentsAsync(ct);
            return Ok(counteragents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка контрагентов");
            return BadRequest($"Ошибка при получении списка контрагентов: {ex.Message}");
        }
    }
}