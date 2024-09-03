using Contracts;

namespace Web.Requests.ProjectRequests;

public class CreateExpenseApiRequest
{
    public required int Project { get; set; }
    public required string Name { get; set; }
    public required double Amount { get; set; }
    public string? Description { get; set; }
    public required ExpenseType Type { get; set; }
    public int? Employee { get; set; }
    public required bool IsPaidByCompany { get; set; }
}