using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services;
using Web.Extensions.ProductExtensions;
using Web.Requests.ProductRequests;
using Web.Responses.ProductResponses;

namespace Web.Controllers.ProductControllers;

[ApiController]
[Route("api/projectProducts")]
public class ProjectProductController : ControllerBase
{
    private readonly IProjectProductService _projectProductService;

    public ProjectProductController(IProjectProductService projectProductService)
    {
        _projectProductService = projectProductService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProjectProduct))]
    public IActionResult CreateProjectProduct([FromBody] CreateProjectProductApiRequest apiRequest)
    {
        var projectProduct = apiRequest.ToProjectProduct();
        var projectProductId = _projectProductService.CreateProjectProduct(projectProduct);
        var createdProjectProduct = _projectProductService.GetProjectProduct(projectProductId);
        return Ok(createdProjectProduct.ToApiProjectProduct());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiProjectProduct))]
    public IActionResult GetProjectProduct(int id)
    {
        var projectProduct = _projectProductService.GetProjectProduct(id);
        return Ok(projectProduct.ToApiProjectProduct());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProjectProduct(int id, [FromBody] UpdateProjectProductApiRequest apiRequest)
    {
        var projectProduct = apiRequest.ToProjectProduct(id);
        _projectProductService.UpdateProjectProduct(projectProduct);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProjectProduct(int id)
    {
        _projectProductService.DeleteProjectProduct(id);
        return Ok();
    }
}