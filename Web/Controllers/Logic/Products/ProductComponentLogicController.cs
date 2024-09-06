using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Services;

namespace Web.Controllers.Logic.Products;

[Authorize]
[ApiController]
[Route("api/productComponents/logic")]
public class ProductComponentLogicController : ControllerBase
{
    private readonly ILogger<ProductComponentLogicController> _logger;
    private readonly IProductComponentService _productComponentService;

    public ProductComponentLogicController(ILogger<ProductComponentLogicController> logger)
    {
        _logger = logger;
    }
}