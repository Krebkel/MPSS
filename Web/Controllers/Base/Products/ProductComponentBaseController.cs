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

    public ProductComponentBaseController(ILogger<ProductComponentBaseController> logger)
    {
        _logger = logger;
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
                createdProductComponent.Product.Name, createdProductComponent.Name);

            return Ok(createdProductComponent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении компонента изделия");
            return BadRequest($"Ошибка при добавлении компонента изделия {ex.Message}");
        }
    }

    [HttpPut]
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
                request.Product.Name, request.Name);
            
            return Ok(updatedProductComponent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о компоненте изделия");
            return BadRequest($"Ошибка при обновлении информации о компоненте изделия {ex.Message}");
        }
    }
}