using Contracts;
using Contracts.ProjectEntities;

namespace Web.Requests.ProjectRequests;

public class CreateExpenseApiRequest
{
    public required Project Project { get; set; }
    public required string Name { get; set; }
    public required double Amount { get; set; }
    public string? Description { get; set; }
    public required ExpenseType Type { get; set; }
    public required bool IsPaidByCompany { get; set; }
}