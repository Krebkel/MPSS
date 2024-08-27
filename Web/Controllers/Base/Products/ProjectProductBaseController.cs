using Contracts.ProductEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
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

    public ProjectProductBaseController(ILogger<ProjectProductBaseController> logger, 
        IProjectProductService projectProductService)
    {
        _logger = logger;
        _projectProductService = projectProductService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProjectProduct>))]
    public async Task<IActionResult> AddProjectProducts(
        [FromBody] IEnumerable<CreateProjectProductApiRequest> requests, CancellationToken ct)
    {
        try
        {
            var createdProjectProducts = new List<ProjectProduct>();

            foreach (var request in requests)
            {
                var addProjectProductRequest = request.ToCreateProjectProductRequest();
                var createdProjectProduct = await _projectProductService
                    .CreateProjectProductAsync(addProjectProductRequest, ct);
                createdProjectProducts.Add(createdProjectProduct);
            
                _logger.LogInformation("Изделие {@ProductName} успешно добавлено на проект {@ProjectName} ",
                    createdProjectProduct.Product.Name, createdProjectProduct.Project.Name);
            }

            return Ok(createdProjectProducts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении изделий на проект");
            return BadRequest($"Ошибка при добавлении изделий на проект: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProjectProduct>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProjectProducts(
        [FromBody] IEnumerable<UpdateProjectProductApiRequest> requests, CancellationToken ct)
    {
        try
        {
            var updatedProjectProducts = new List<ProjectProduct>();

            foreach (var request in requests)
            {
                var updateProjectProductRequest = request.ToUpdateProjectProductRequest();
                var updatedProjectProduct = await _projectProductService
                    .UpdateProjectProductAsync(updateProjectProductRequest, ct);
                updatedProjectProducts.Add(updatedProjectProduct);

                _logger.LogInformation("Изделие {@ProductName} успешно обновлено на проекте {@ProjectName}",
                    updatedProjectProduct.Product.Name, updatedProjectProduct.Project.Name);
            }

            return Ok(updatedProjectProducts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации об изделиях на проекте");
            return BadRequest($"Ошибка при обновлении информации об изделиях на проекте: {ex.Message}");
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProjectProduct(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _projectProductService.DeleteProjectProductAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Проектного изделия с ID {Id} не найден", id);
                return NotFound($"Проектное изделие с ID {id} не найден");
            }

            _logger.LogInformation("Проектное изделие с ID {Id} успешно удален", id);
            return Ok();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23503")
        {
            _logger.LogError(ex, "Ошибка при удалении проектного изделия из-за внешних ключей");
            return BadRequest("Невозможно удалить проектного изделия, так как он связан с другими записями.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении изделия на проекте");
            return BadRequest($"Ошибка при удалении изделия на проекте: {ex.Message}");
        }
    }
    
    [HttpGet("byProject/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProjectProduct>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectProductsByProjectId(int projectId, CancellationToken ct)
    {
        try
        {
            var projectProducts = await _projectProductService
                .GetProjectProductsByProjectIdAsync(projectId, ct);
        
            if (projectProducts == null)
            {
                _logger.LogWarning("Проект с ID {ProjectId} не найден", projectId);
                return NotFound($"Проект с ID {projectId} не найден");
            }

            return Ok(projectProducts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении изделий по ID проекта");
            return BadRequest($"Ошибка при получении изделий по ID проекта: {ex.Message}");
        }
    }
}