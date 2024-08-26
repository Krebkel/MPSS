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
[Route("api/products/base")]
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
    public async Task<IActionResult> AddProduct([FromBody] CreateProductApiRequest request, CancellationToken ct)
    {
        try
        {
            var addProductRequest = request.ToCreateProductApiRequest();
            var createdProduct = await _productService.CreateProductAsync(addProductRequest, ct);
            
            _logger.LogInformation("Изделие успешно добавлено: {@Name}", createdProduct.Name);

            return Ok(createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении изделия");
            return BadRequest($"Ошибка при добавлении изделия {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatedProduct = request.ToUpdateProductApiRequest();

            await _productService.UpdateProductAsync(updatedProduct, ct);

            _logger.LogInformation("Изделие успешно обновлено: {@Name}", request.Name);
            
            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении информации об изделии");
            return BadRequest($"Ошибка при обновлении информации об изделии {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int id, CancellationToken ct)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id, ct);
            if (product == null)
            {
                _logger.LogWarning("Изделие с ID {Id} не найдено", id);
                return NotFound($"Изделие с ID {id} не найдено");
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных об изделии");
            return BadRequest($"Ошибка при получении данных об изделии: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken ct)
    {
        try
        {
            var deleteResult = await _productService.DeleteProductAsync(id, ct);
            if (!deleteResult)
            {
                _logger.LogWarning("Изделие с ID {Id} не найдено", id);
                return NotFound($"Изделин с ID {id} не найдено");
            }

            _logger.LogInformation("Изделие с ID {Id} успешно удалено", id);
            return Ok();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23503")
        {
            _logger.LogError(ex, "Ошибка при удалении изделия из-за внешних ключей");
            return BadRequest("Невозможно удалить изделие, так как он связан с другими записями.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении изделия");
            return BadRequest($"Ошибка при удалении изделия: {ex.Message}");
        }
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    public async Task<IActionResult> GetAllProducts(CancellationToken ct)
    {
        try
        {
            var products = await _productService.GetAllProductsAsync(ct);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка изделий");
            return BadRequest($"Ошибка при получении списка изделий: {ex.Message}");
        }
    }
}