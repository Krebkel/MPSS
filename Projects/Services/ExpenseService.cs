using Contracts.EmployeeEntities;
using Microsoft.Extensions.Logging;
using Contracts.ProjectEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Projects.Services;

public class ExpenseService : IExpenseService
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly ILogger<ExpenseService> _logger;
    private readonly IValidator<Project> _projectValidator;
    private readonly IValidator<Employee> _employeeValidator;
    private readonly IValidator<Expense> _expenseValidator;

    public ExpenseService(
        IRepository<Expense> expenseRepository,
        IRepository<Project> projectRepository,
        IRepository<Employee> employeeRepository,
        ILogger<ExpenseService> logger,
        IValidator<Project> projectValidator,
        IValidator<Expense> expenseValidator,
        IValidator<Employee> employeeValidator)
    {
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
        _expenseRepository = expenseRepository;
        _logger = logger;
        _projectValidator = projectValidator;
        _employeeValidator = employeeValidator;
        _expenseValidator = expenseValidator;
    }


    public async Task<Expense> CreateExpenseAsync(CreateExpenseRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var project = await _projectValidator.ValidateAndGetEntityAsync(
            request.Project, _projectRepository, "Проект", cancellationToken);

        Employee? employee = null;
        if (request.Employee.HasValue)
        {
            employee = await _employeeValidator.ValidateAndGetEntityAsync(
                request.Employee,
                _employeeRepository,
                "Сотрудник",
                cancellationToken);
        }

        var createdExpense = new Expense
        {
            Project = project,
            Name = request.Name,
            Amount = request.Amount,
            Description = request.Description,
            Type = request.Type,
            Employee = employee,
            IsPaidByCompany = request.IsPaidByCompany
        };

        await _expenseRepository.AddAsync(createdExpense, cancellationToken);
        _logger.LogInformation("Расход успешно добавлен: {@Expense}", createdExpense);

        return createdExpense;
    }
    
    public async Task<Expense> UpdateExpenseAsync(UpdateExpenseRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var expense = await _expenseValidator.ValidateAndGetEntityAsync(
            request.Id, _expenseRepository, "Расход", cancellationToken);

        var project = await _projectValidator.ValidateAndGetEntityAsync(
            request.Project, _projectRepository, "Проект", cancellationToken);

        Employee? employee = null;
        if (request.Employee.HasValue)
        {
            employee = await _employeeValidator.ValidateAndGetEntityAsync(
                request.Employee.Value,
                _employeeRepository,
                "Контрагент",
                cancellationToken);
        }

        expense.Project = project;
        expense.Name = request.Name;
        expense.Amount = request.Amount;
        expense.Description = request.Description;
        expense.Type = request.Type;
        expense.Employee = employee;  
        expense.IsPaidByCompany = request.IsPaidByCompany;
        
        await _expenseRepository.UpdateAsync(expense, cancellationToken);
        _logger.LogInformation("Расход успешно обновлен: {@Expense}", expense);

        return expense;
    }

    public async Task<object?> GetExpenseByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _expenseRepository
            .GetAll()
            .Include(e => e.Project)
            .Include(e => e.Employee)
            .Select(e => new
            {
                Id = e.Id,
                Project = e.Project.Id,
                Name = e.Name,
                Amount = e.Amount,
                Description = e.Description,
                Type = e.Type,
                Employee = e.Employee != null ? e.Employee.Id : (int?)null,
                IsPaidByCompany = e.IsPaidByCompany
            })
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
    
    public async Task<IEnumerable<object>> GetExpensesByProjectIdAsync(
        int projectId, CancellationToken cancellationToken)
    {
        try {
            var projectExists = await _projectRepository.GetAll()
                .AnyAsync(p => p.Id == projectId, cancellationToken);

            if (!projectExists)
            {
                _logger.LogWarning("Проект с ID {ProjectId} не найден", projectId);
                throw new KeyNotFoundException($"Проект с ID {projectId} не найден");
            }
            
            return await _expenseRepository
                .GetAll()
                .Include(e => e.Project)
                .Include(e => e.Employee)
                .Where(e => e.Project.Id == projectId)
                .Select(e => new
                {
                    Id = e.Id,
                    Project = e.Project.Id,
                    Name = e.Name,
                    Amount = e.Amount,
                    Description = e.Description,
                    Type = e.Type,
                    Employee = e.Employee != null ? e.Employee.Id : (int?)null,
                    IsPaidByCompany = e.IsPaidByCompany
                })
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении расходов для проекта с ID {ProjectId}",
                projectId);
            throw;
        }
    }
}