using Contracts.ProductEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Services;
using Web.Extensions.ProductExtensions;
using Web.Requests.ProductRequests;

namespace Web.Controllers.Base.Products;

[Authorize]
[ApiController]
[Route("api/components")]
public class ComponentController : ControllerBase
{
    private readonly ILogger<ComponentController> _logger;
    private readonly IComponentService _componentService;

    public ComponentController(ILogger<ComponentController> logger, 
        IComponentService componentService)
    {
        _logger = logger;
        _componentService = componentService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Component))]
    public async Task<IActionResult> CreateComponent(
        [FromBody] CreateComponentApiRequest request, CancellationToken ct)
    {
        try
        {
            var createComponentRequest = request.ToCreateComponentRequest();
            var createdComponent = await _componentService.CreateComponentAsync(createComponentRequest, ct);

            _logger.LogInformation("Компонент {@ComponentName} успешно добавлен", createdComponent.Name);

            return Ok(createdComponent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении компонента");
            return BadRequest($"Ошибка при добавлении компонента: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Component))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateComponent(
        [FromBody] UpdateComponentApiRequest request, CancellationToken ct)
    {
        try
        {
            var updateComponentRequest = request.ToUpdateComponentRequest();
            var updatedComponent = await _componentService.UpdateComponentAsync(updateComponentRequest, ct);

            _logger.LogInformation("Компонент {@ComponentName} успешно обновлен", updatedComponent.Name);
            
            return Ok(updatedComponent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о компоненте");
            return BadRequest($"Ошибка при обновлении информации о компоненте: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Component))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComponent(int id, CancellationToken ct)
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id, ct);
            if (component == null)
            {
                _logger.LogWarning("Компонент с ID {Id} не найден", id);
                return NotFound($"Компонент с ID {id} не найден");
            }

            return Ok(component);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о компоненте");
            return BadRequest($"Ошибка при получении данных о компоненте: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteComponent(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _componentService.DeleteComponentAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Компонент с ID {Id} не найден", id);
                return NotFound($"Компонент с ID {id} не найден");
            }

            _logger.LogInformation("Компонент с ID {Id} успешно удален", id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении компонента");
            return BadRequest($"Ошибка при удалении компонента: {ex.Message}");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Component>))]
    public async Task<IActionResult> GetAllComponents(CancellationToken ct)
    {
        try
        {
            var components = await _componentService.GetAllComponentsAsync(ct);
            return Ok(components);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка компонентов");
            return BadRequest($"Ошибка при получении списка компонентов: {ex.Message}");
        }
    }
}