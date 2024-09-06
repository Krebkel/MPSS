using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projects.Services;

namespace Web.Controllers.Logic.Projects;

[Authorize]
[ApiController]
[Route("api/expenses/logic")]
public class ExpenseLogicController : ControllerBase
{
    private readonly ILogger<ExpenseLogicController> _logger;
    private readonly IExpenseService _expenseService;

    public ExpenseLogicController(ILogger<ExpenseLogicController> logger)
    {
        _logger = logger;
    }
}