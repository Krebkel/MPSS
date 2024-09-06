using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Services;

namespace Web.Controllers.Logic.Products;

[Authorize]
[ApiController]
[Route("api/projectProducts/logic")]
public class ProjectProductLogicController : ControllerBase
{
    private readonly ILogger<ProjectProductLogicController> _logger;
    private readonly IProjectProductService _projectProductService;

    public ProjectProductLogicController(ILogger<ProjectProductLogicController> logger)
    {
        _logger = logger;
    }
}