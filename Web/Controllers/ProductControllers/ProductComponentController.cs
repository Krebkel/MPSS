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
        try
        {
            var productComponent = apiRequest.ToProductComponent();
            var productComponentId = _productComponentService.CreateProductComponent(productComponent);
            var createdProductComponent = _productComponentService.GetProductComponent(productComponentId);
            return Ok(createdProductComponent.ToApiProductComponent());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при создании расхода");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProductComponent))]
    public IActionResult GetProductComponent(int id)
    {
        var productComponent = _productComponentService.GetProductComponent(id);
        return Ok(productComponent.ToApiProductComponent());
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApiProductComponent>))]
    public IActionResult GetAllProductComponents()
    {
        var productComponents = _productComponentService.GetAllProductComponents();
        
        var response = productComponents.Select(es => new ApiProductComponent()
        {
            Id = es.Id,
            Name = es.Name,
            ProductId = es.ProductId,
            Quantity = es.Quantity,
            Weight = es.Weight
        }).ToList();        
        
        return Ok(response);
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