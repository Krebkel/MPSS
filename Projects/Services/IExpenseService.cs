using Contracts;
using Contracts.ProjectEntities;

namespace Projects.Services;

public interface IExpenseService
{
    Task<Expense> CreateExpenseAsync(CreateExpenseRequest expense, CancellationToken cancellationToken);
    
    Task<Expense> UpdateExpenseAsync(UpdateExpenseRequest expense, CancellationToken cancellationToken);
    
    Task<object?> GetExpenseByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteExpenseAsync(int id, CancellationToken cancellationToken);
    
    Task<IEnumerable<object>> GetExpensesByProjectIdAsync(int projectId, CancellationToken cancellationToken);
}

public class UpdateExpenseRequest
{
    public int Id { get; set; }
    public int Project { get; set; }
    public string Name { get; set; }
    public double? Amount { get; set; }
    public string? Description { get; set; }
    public ExpenseType Type { get; set; }
    public int? Employee { get; set; }
    public bool IsPaidByCompany { get; set; }
}

public class CreateExpenseRequest
{
    public required int Project { get; set; }
    public required string Name { get; set; }
    public required double Amount { get; set; }
    public string? Description { get; set; }
    public required ExpenseType Type { get; set; }
    public int? Employee { get; set; }
    public required bool IsPaidByCompany { get; set; }
}