using Employees.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Logic.Employees;

[Authorize]
[ApiController]
[Route("api/employees/logic")]
public class EmployeeLogicController : ControllerBase
{
    private readonly ILogger<EmployeeLogicController> _logger;
    private readonly IEmployeeService _employeeService;

    public EmployeeLogicController(ILogger<EmployeeLogicController> logger)
    {
        _logger = logger;
    }
}