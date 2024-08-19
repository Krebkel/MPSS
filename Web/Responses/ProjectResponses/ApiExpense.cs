using Contracts.ProjectEntities;

namespace Web.Responses.ProjectResponses;

public class ApiExpense
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public double? Amount { get; set; }
    public string? Description { get; set; }
    public ExpenseType Type { get; set; }
    public bool IsPaidByCompany { get; set; }
}