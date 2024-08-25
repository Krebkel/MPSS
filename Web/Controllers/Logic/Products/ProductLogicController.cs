using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Services;

namespace Web.Controllers.Logic.Products;

[ApiController]
[Route("api/products/logic")]
public class ProductLogicController : ControllerBase
{
    private readonly ILogger<ProductLogicController> _logger;
    private readonly IProductService _productService;

    public ProductLogicController(ILogger<ProductLogicController> logger)
    {
        _logger = logger;
    }
}