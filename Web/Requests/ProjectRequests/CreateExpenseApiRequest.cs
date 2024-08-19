using Contracts.ProjectEntities;

namespace Web.Requests.ProjectRequests;

public class CreateExpenseApiRequest
{
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public double? Amount { get; set; }
    public string? Description { get; set; }
    public ExpenseType Type { get; set; }
    public bool IsPaidByCompany { get; set; }
}