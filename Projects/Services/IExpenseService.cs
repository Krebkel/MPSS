using Contracts.ProjectEntities;

namespace Projects.Services;

public interface IExpenseService
{
    int CreateExpense(Expense expense);
    
    Expense GetExpense(int expenseId);
    
    List<Expense> GetExpensesByProject(int projectId);
    
    void UpdateExpense(Expense expense);
    
    void DeleteExpense(int expenseId);
}