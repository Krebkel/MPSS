using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projects.Services;
using Web.Extensions.ProjectExtensions;
using Web.Requests.ProjectRequests;
using Web.Responses.ProjectResponses;

namespace Web.Controllers.ProjectControllers;

[ApiController]
[Route("api/expenses")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiExpense))]
    public IActionResult CreateExpense([FromBody] CreateExpenseApiRequest apiRequest)
    {
        var expense = apiRequest.ToExpense();
        var expenseId = _expenseService.CreateExpense(expense);
        var createdExpense = _expenseService.GetExpense(expenseId);
        return Ok(createdExpense.ToApiExpense());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiExpense))]
    public IActionResult GetExpense(int id)
    {
        var expense = _expenseService.GetExpense(id);
        return Ok(expense.ToApiExpense());
    }

    [HttpGet("project/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApiExpense>))]
    public IActionResult GetExpensesByProject(int projectId)
    {
        var expenses = _expenseService.GetExpensesByProject(projectId);
        return Ok(expenses.Select(e => e.ToApiExpense()).ToList());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExpense(int id, [FromBody] UpdateExpenseApiRequest apiRequest)
    {
        var expense = apiRequest.ToExpense(id);
        _expenseService.UpdateExpense(expense);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteExpense(int id)
    {
        _expenseService.DeleteExpense(id);
        return Ok();
    }
}