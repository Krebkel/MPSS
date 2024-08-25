using Contracts.ProductEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Services;
using Web.Extensions.ProductExtensions;
using Web.Requests.ProductRequests;

namespace Web.Controllers.Base.Products;

[ApiController]
[Route("api/projectProducts/base")]
public class ProjectProductBaseController : ControllerBase
{
    private readonly ILogger<ProjectProductBaseController> _logger;
    private readonly IProjectProductService _projectProductService;

    public ProjectProductBaseController(ILogger<ProjectProductBaseController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectProduct))]
    public async Task<IActionResult> AddProjectProduct(
        [FromBody] CreateProjectProductApiRequest request, CancellationToken ct)
    {
        try
        {
            var addProjectProductRequest = request.ToCreateProjectProductRequest();
            var createdProjectProduct = await _projectProductService
                .CreateProjectProductAsync(addProjectProductRequest, ct);
            
            _logger.LogInformation("Изделие {@ProductName} успешно добавлено на проект {@ProjectName} ",
                createdProjectProduct.Product.Name, createdProjectProduct.Project.Name);

            return Ok(createdProjectProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении изделия");
            return BadRequest($"Ошибка при добавлении изделия {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectProduct))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProjectProduct(
        [FromBody] UpdateProjectProductApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedProjectProduct = request.ToUpdateProjectProductRequest();

            await _projectProductService.UpdateProjectProductAsync(updatedProjectProduct, ct);

            _logger.LogInformation("Изделие {@ProductName} успешно обновлено на проекте {@ProjectName}",
                request.Product.Name, request.Project.Name);
            
            return Ok(updatedProjectProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации об изделии на проекте");
            return BadRequest($"Ошибка при обновлении информации об изделии на проекте{ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectProduct))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectProduct(int id, CancellationToken ct)
    {
        try
        {
            var projectProduct = await _projectProductService.GetProjectProductByIdAsync(id, ct);
            if (projectProduct == null)
            {
                _logger.LogWarning("Сотрудник с ID {Id} не найден", id);
                return NotFound($"Сотрудник с ID {id} не найден");
            }

            return Ok(projectProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о компоненте изделия");
            return BadRequest($"Ошибка при получении данных о компоненте изделия: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProjectProduct(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _projectProductService.DeleteProjectProductAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Компонент изделия с ID {Id} не найден", id);
                return NotFound($"Сотрудник с ID {id} не найден");
            }

            _logger.LogInformation("Компонент изделия с ID {Id} успешно удален", id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении компонента изделия");
            return BadRequest($"Ошибка при удалении компонента изделия: {ex.Message}");
        }
    }
}