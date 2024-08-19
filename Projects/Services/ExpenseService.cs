using Contracts.ProjectEntities;
using Data;

namespace Projects.Services;

public class ExpenseService : IExpenseService
{
    private readonly AppDbContext _context;

    public ExpenseService(AppDbContext context)
    {
        _context = context;
    }

    public int CreateExpense(Expense expense)
    {
        _context.Expenses.Add(expense);
        _context.SaveChanges();
        return expense.Id;
    }

    public Expense GetExpense(int expenseId)
    {
        return _context.Expenses.Find(expenseId);
    }

    public List<Expense> GetExpensesByProject(int projectId)
    {
        return _context.Expenses.Where(e => e.ProjectId == projectId).ToList();
    }

    public void UpdateExpense(Expense expense)
    {
        var existingExpense = _context.Expenses.Find(expense.Id);
        if (existingExpense != null)
        {
            _context.Expenses.Update(expense);
            _context.SaveChanges();
        }
    }

    public void DeleteExpense(int expenseId)
    {
        var expense = _context.Expenses.Find(expenseId);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
        }
    }
}