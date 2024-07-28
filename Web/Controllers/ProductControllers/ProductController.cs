using Products.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions.ProductExtensions;
using Web.Requests;
using Web.Requests.ProductRequests;
using Web.Responses;
using Web.Responses.ProductResponses;

namespace Web.Controllers.ProductControllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProduct))]
    public IActionResult CreateProduct([FromBody] CreateProductApiRequest apiRequest)
    {
        var product = apiRequest.ToProduct();
        var productId = _productService.CreateProduct(product);
        var createdProduct = _productService.GetProduct(productId);
        return Ok(createdProduct.ToApiProduct());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProduct))]
    public IActionResult GetProduct(int id)
    {
        var product = _productService.GetProduct(id);
        return Ok(product.ToApiProduct());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] UpdateProductApiRequest apiRequest)
    {
        var product = apiRequest.ToProduct(id);
        _productService.UpdateProduct(product);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        _productService.DeleteProduct(id);
        return Ok();
    }
}