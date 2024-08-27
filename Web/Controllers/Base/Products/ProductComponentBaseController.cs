using Contracts.ProductEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Services;
using Web.Extensions.ProductExtensions;
using Web.Requests.ProductRequests;

namespace Web.Controllers.Base.Products;

[ApiController]
[Route("api/productComponents/base")]
public class ProductComponentBaseController : ControllerBase
{
    private readonly ILogger<ProductComponentBaseController> _logger;
    private readonly IProductComponentService _productComponentService;

    public ProductComponentBaseController(ILogger<ProductComponentBaseController> logger, 
        IProductComponentService productComponentService)
    {
        _logger = logger;
        _productComponentService = productComponentService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductComponent>))]
    public async Task<IActionResult> AddProductComponents(
        [FromBody] IEnumerable<CreateProductComponentApiRequest> requests, CancellationToken ct)
    {
        try
        {
            var createdProductComponents = new List<ProductComponent>();

            foreach (var request in requests)
            {
                var addProductComponentRequest = request.ToCreateProductComponentApiRequest();
                var createdProductComponent = await _productComponentService
                    .CreateProductComponentAsync(addProductComponentRequest, ct);
                createdProductComponents.Add(createdProductComponent);
            
                _logger.LogInformation("Компонент изделия {@ProductName} успешно добавлен: {@Name}",
                    createdProductComponent.Product, createdProductComponent.Name);
            }

            return Ok(createdProductComponents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении компонентов изделия");
            return BadRequest($"Ошибка при добавлении компонентов изделия: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductComponent>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductComponents(
        [FromBody] IEnumerable<UpdateProductComponentApiRequest> requests, CancellationToken ct)
    {
        try
        {
            var updatedProductComponents = new List<ProductComponent>();

            foreach (var request in requests)
            {
                var updateProductComponentRequest = request.ToUpdateProductComponentApiRequest();
                var updatedProductComponent = await _productComponentService
                    .UpdateProductComponentAsync(updateProductComponentRequest, ct);
                updatedProductComponents.Add(updatedProductComponent);

                _logger.LogInformation("Компонент изделия {@ProductName} успешно обновлен: {@Name}", 
                    updatedProductComponent.Product, updatedProductComponent.Name);
            }

            return Ok(updatedProductComponents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о компонентах изделия");
            return BadRequest($"Ошибка при обновлении информации о компонентах изделия: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductComponent(int id, CancellationToken ct)
    {
        try
        {
            var productComponent = await _productComponentService.GetProductComponentByIdAsync(id, ct);
            if (productComponent == null)
            {
                _logger.LogWarning("Компонент изделия с ID {Id} не найден", id);
                return NotFound($"Компонент изделия с ID {id} не найден");
            }

            return Ok(productComponent);
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
    public async Task<IActionResult> DeleteProductComponent(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _productComponentService.DeleteProductComponentAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Компонент изделия с ID {Id} не найден", id);
                return NotFound($"Компонент изделия с ID {id} не найден");
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
    
    [HttpGet("byProduct/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductComponent>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductComponentsByProductId(int productId, CancellationToken ct)
    {
        try
        {
            var productComponents = await _productComponentService.GetProductComponentsByProductIdAsync(productId, ct);
            return Ok(productComponents);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении компонентов по ID изделия");
            return BadRequest($"Ошибка при получении компонентов по ID изделия: {ex.Message}");
        }
    }
}