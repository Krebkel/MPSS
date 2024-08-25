using Contracts;
using Microsoft.Extensions.Logging;
using Contracts.ProjectEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Projects.Services;

public class ExpenseService : IExpenseService
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly ILogger<ExpenseService> _logger;

    public ExpenseService(
        IRepository<Expense> expenseRepository,
        ILogger<ExpenseService> logger)
    {
        _expenseRepository = expenseRepository;
        _logger = logger;
    }

    public async Task<Expense> CreateExpenseAsync(CreateExpenseRequest request, CancellationToken cancellationToken)
    {
        var createdExpense = new Expense
        {
            Project = request.Project,
            Name = request.Name,
            Amount = request.Amount,
            Description = request.Description,
            Type = request.Type,
            IsPaidByCompany = request.IsPaidByCompany
        };
        
        _logger.LogInformation("Расход успешно добавлен: {@Expense}", createdExpense);
        return createdExpense;
    }
    
    public async Task<Expense?> UpdateExpenseAsync(UpdateExpenseRequest request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (expense == null) return null;

        expense.Id = request.Id;
        expense.Project = request.Project;
        expense.Name = request.Name;
        expense.Amount = request.Amount;
        expense.Description = request.Description;
        expense.Type = request.Type;
        expense.IsPaidByCompany = request.IsPaidByCompany;

        await _expenseRepository.UpdateAsync(expense, cancellationToken);

        _logger.LogInformation("Расход успешно обновлен: {@Expense}", expense);
        return expense;
    }
    
    public async Task<Expense?> GetExpenseByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _expenseRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteExpenseAsync(int id, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (expense == null) return false;

        await _expenseRepository.DeleteAsync(expense, cancellationToken);
        _logger.LogInformation("Расход с ID {Id} успешно удален", id);
        return true;
    }
}