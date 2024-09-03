using Contracts.ProductEntities;
using Microsoft.AspNetCore.Authorization;
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
[Route("api/products/base")]
[Authorize]
public class ProductBaseController : ControllerBase
{
    private readonly ILogger<ProductBaseController> _logger;
    private readonly IProductService _productService;

    public ProductBaseController(ILogger<ProductBaseController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductApiRequest request, CancellationToken ct)
    {
        try
        {
            var addProductRequest = request.ToCreateProductApiRequest();
            var createdProduct = await _productService.CreateProductAsync(addProductRequest, ct);
            
            _logger.LogInformation("Работа успешно добавлено: {@Name}", createdProduct.Name);

            return Ok(createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении работы");
            return BadRequest($"Ошибка при добавлении работы {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedProduct = request.ToUpdateProductApiRequest();

            await _productService.UpdateProductAsync(updatedProduct, ct);

            _logger.LogInformation("Работа успешно обновлена: {@Name}", request.Name);
            
            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации о работе");
            return BadRequest($"Ошибка при обновлении информации о работе {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> GetProduct(int id, CancellationToken ct)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id, ct);
            if (product == null)
            {
                _logger.LogWarning("Работа с ID {Id} не найдена", id);
                return NotFound($"Работа с ID {id} не найдена");
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о работе");
            return BadRequest($"Ошибка при получении данных о работе: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _productService.DeleteProductAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Работа с ID {Id} не найдена", id);
                return NotFound($"Работа с ID {id} не найдена");
            }

            _logger.LogInformation("Работа с ID {Id} успешно удалена", id);
            return Ok();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23503")
        {
            _logger.LogError(ex, "Ошибка при удалении работы из-за внешних ключей");
            return BadRequest("Невозможно удалить работу, так как она связан с другими записями.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении работы");
            return BadRequest($"Ошибка при удалении работы: {ex.Message}");
        }
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    [Authorize(Roles = "Service,Administrator")]
    public async Task<IActionResult> GetAllProducts(CancellationToken ct)
    {
        try
        {
            var products = await _productService.GetAllProductsAsync(ct);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка работ");
            return BadRequest($"Ошибка при получении списка работ: {ex.Message}");
        }
    }
}