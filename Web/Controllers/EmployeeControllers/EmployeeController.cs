using Employees.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions.EmployeeExtensions;
using Web.Requests.EmployeeRequests;
using Web.Responses.EmployeeResponses;

namespace Web.Controllers.EmployeeControllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiEmployee))]
    public IActionResult CreateEmployee([FromBody] CreateEmployeeApiRequest apiRequest)
    {
        var employee = apiRequest.ToEmployee();
        var employeeId = _employeeService.CreateEmployee(employee);
        var createdEmployee = _employeeService.GetEmployee(employeeId);
        return Ok(createdEmployee.ToApiEmployee());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiEmployee))]
    public IActionResult GetEmployee(int id)
    {
        var employee = _employeeService.GetEmployee(id);
        return Ok(employee.ToApiEmployee());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEmployee(int id, [FromBody] UpdateEmployeeApiRequest apiRequest)
    {
        var employee = apiRequest.ToEmployee(id);
        _employeeService.UpdateEmployee(employee);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        _employeeService.DeleteEmployee(id);
        return Ok();
    }
}