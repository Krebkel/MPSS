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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductComponent))]
    public async Task<IActionResult> AddProductComponent(
        [FromBody] CreateProductComponentApiRequest request, CancellationToken ct)
    {
        try
        {
            var addProductComponentRequest = request.ToCreateProductComponentApiRequest();
            var createdProductComponent = await _productComponentService
                .CreateProductComponentAsync(addProductComponentRequest, ct);
            
            _logger.LogInformation("Компонент изделия {@ProductName} успешно добавлен: {@Name}",
                createdProductComponent.Product, createdProductComponent.Name);

            return Ok(createdProductComponent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении компонента изделия");
            return BadRequest($"Ошибка при добавлении компонента изделия {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductComponent))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductComponent(
        [FromBody] UpdateProductComponentApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedProductComponent = request.ToUpdateProductComponentApiRequest();

            await _productComponentService.UpdateProductComponentAsync(updatedProductComponent, ct);

            _logger.LogInformation("Компонент изделия {@ProductName} успешно обновлен: {@Name}", 
                request.Product, request.Name);
            
            return Ok(updatedProductComponent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о компоненте изделия");
            return BadRequest($"Ошибка при обновлении информации о компоненте изделия {ex.Message}");
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
            var productComponents = 
                await _productComponentService.GetProductComponentsByProductIdAsync(productId, ct);
            if (!productComponents.Any())
            {
                _logger.LogWarning("Изделие с ID {ProductId} не найдено или нет компонентов изделия", productId);
                return NotFound($"Изделие с ID {productId} не найдено или нет компонентов изделия");
            }

            return Ok(productComponents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении компонентов по ID изделия");
            return BadRequest($"Ошибка при получении компонентов по ID изделия: {ex.Message}");
        }
    }
}