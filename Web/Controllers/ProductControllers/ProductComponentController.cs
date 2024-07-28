using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services;
using Web.Extensions.ProductExtensions;
using Web.Requests.ProductRequests;
using Web.Responses.ProductResponses;

namespace Web.Controllers.ProductControllers;

[ApiController]
[Route("api/productComponents")]
public class ProductComponentController : ControllerBase
{
    private readonly IProductComponentService _productComponentService;

    public ProductComponentController(IProductComponentService productComponentService)
    {
        _productComponentService = productComponentService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProductComponent))]
    public IActionResult CreateProductComponent([FromBody] CreateProductComponentApiRequest apiRequest)
    {
        var productComponent = apiRequest.ToProductComponent();
        var productComponentId = _productComponentService.CreateProductComponent(productComponent);
        var createdProductComponent = _productComponentService.GetProductComponent(productComponentId);
        return Ok(createdProductComponent.ToApiProductComponent());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProductComponent))]
    public IActionResult GetProductComponent(int id)
    {
        var productComponent = _productComponentService.GetProductComponent(id);
        return Ok(productComponent.ToApiProductComponent());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProductComponent(int id, [FromBody] UpdateProductComponentApiRequest apiRequest)
    {
        var productComponent = apiRequest.ToProductComponent(id);
        _productComponentService.UpdateProductComponent(productComponent);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProductComponent(int id)
    {
        _productComponentService.DeleteProductComponent(id);
        return Ok();
    }

    [HttpGet("calculateTotalWeight/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
    public IActionResult CalculateTotalWeight(int productId)
    {
        var totalWeight = _productComponentService.CalculateTotalWeight(productId);
        return Ok(totalWeight);
    }
}