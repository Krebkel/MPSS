using Employees.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Logic.Employees;

[Authorize]
[ApiController]
[Route("api/employeeShifts/logic")]
public class EmployeeShiftLogicController : ControllerBase
{
    private readonly ILogger<EmployeeShiftLogicController> _logger;
    private readonly IEmployeeShiftService _employeeShiftService;

    public EmployeeShiftLogicController(ILogger<EmployeeShiftLogicController> logger)
    {
        _logger = logger;
    }
}