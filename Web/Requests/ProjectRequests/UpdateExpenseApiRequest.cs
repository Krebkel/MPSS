using Contracts;

namespace Web.Requests.ProjectRequests;

public class UpdateExpenseApiRequest
{
    public required int Id { get; set; }
    public int Project { get; set; }
    public string Name { get; set; }
    public double? Amount { get; set; }
    public string? Description { get; set; }
    public ExpenseType Type { get; set; }
    public int? Employee { get; set; }
    public bool IsPaidByCompany { get; set; }
}