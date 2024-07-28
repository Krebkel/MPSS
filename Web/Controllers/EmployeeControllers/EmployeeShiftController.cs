using Employees.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions.EmployeeExtensions;
using Web.Requests.EmployeeRequests;
using Web.Responses.EmployeeResponses;

namespace Web.Controllers.EmployeeControllers;

[ApiController]
[Route("api/employeeShifts")]
public class EmployeeShiftController : ControllerBase
{
    private readonly IEmployeeShiftService _employeeShiftService;

    public EmployeeShiftController(IEmployeeShiftService employeeShiftService)
    {
        _employeeShiftService = employeeShiftService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiEmployeeShift))]
    public IActionResult CreateEmployeeShift([FromBody] CreateEmployeeShiftApiRequest apiRequest)
    {
        var employeeShift = apiRequest.ToEmployeeShift();
        var employeeShiftId = _employeeShiftService.CreateEmployeeShift(employeeShift);
        var createdEmployeeShift = _employeeShiftService.GetEmployeeShift(employeeShiftId);
        return Ok(createdEmployeeShift.ToApiEmployeeShift());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEmployeeShift(int id, [FromBody] UpdateEmployeeShiftApiRequest apiRequest)
    {
        var employeeShift = apiRequest.ToEmployeeShift(id);
        _employeeShiftService.UpdateEmployeeShift(employeeShift);
        return Ok();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiEmployeeShift))]
    public IActionResult GetEmployeeShift(int id)
    {
        var employeeShift = _employeeShiftService.GetEmployeeShift(id);
        return Ok(employeeShift.ToApiEmployeeShift());
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployeeShift(int id)
    {
        _employeeShiftService.DeleteEmployeeShift(id);
        return Ok();
    }
}