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
        ValidateExpense(expense);
        
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
        ValidateExpense(expense);
        
        var existingExpense = _context.Expenses.Find(expense.Id);
    
        if (existingExpense != null)
        {
            existingExpense.ProjectId = expense.ProjectId;
            existingExpense.Name = expense.Name ?? existingExpense.Name;
            existingExpense.Amount = expense.Amount ?? existingExpense.Amount;
            existingExpense.Description = expense.Description ?? existingExpense.Description;
            existingExpense.Type = expense.Type;
            existingExpense.IsPaidByCompany = expense.IsPaidByCompany;

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
    
    private void ValidateExpense(Expense expense)
    {
        if (expense.ProjectId <= 0)
        {
            throw new ArgumentException("Ошибка выбора проекта.");
        }

        if (string.IsNullOrWhiteSpace(expense.Name))
        {
            throw new ArgumentException("Ошибка в наименовании затрат");
        }

        if (expense.Amount.HasValue && expense.Amount <= 0)
        {
            throw new ArgumentException("Некорректное значение количества потраченных средств.");
        }

        if (!Enum.IsDefined(typeof(ExpenseType), expense.Type))
        {
            throw new ArgumentException("Ошибка в типе затрат.");
        }
    }
}