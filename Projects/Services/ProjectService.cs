using Contracts.EmployeeEntities;
using Contracts.ProductEntities;
using Microsoft.Extensions.Logging;
using Contracts.ProjectEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Projects.Services;

/// <summary>
/// Сервис для управления логикой проектов
/// </summary>
public class ProjectService : IProjectService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Counteragent> _counteragentRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly IRepository<EmployeeShift> _employeeShiftRepository;
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IRepository<ProjectProduct> _projectProductRepository;
    private readonly ILogger<ProjectService> _logger;
    private readonly IValidator<Employee> _employeeValidator;
    private readonly IValidator<Counteragent> _counteragentValidator;
    private readonly IValidator<Project> _projectValidator;
    
    public ProjectService(
        ILogger<ProjectService> logger,
        IRepository<Project> projectRepository,
        IRepository<Counteragent> counteragentRepository,
        IRepository<Employee> employeeRepository,
        IRepository<EmployeeShift> employeeShiftRepository,
        IRepository<Expense> expenseRepository,
        IRepository<ProjectProduct> projectProductRepository,
        IValidator<Project> projectValidator,
        IValidator<Counteragent> counteragentValidator,
        IValidator<Employee> employeeValidator)
    {
        _logger = logger;
        _projectRepository = projectRepository;
        _projectProductRepository = projectProductRepository;
        _counteragentRepository = counteragentRepository;
        _employeeRepository = employeeRepository;
        _employeeShiftRepository = employeeShiftRepository;
        _expenseRepository = expenseRepository;
        _projectValidator = projectValidator;
        _counteragentValidator = counteragentValidator;
        _employeeValidator = employeeValidator;
    }

    public async Task<Project> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var responsibleEmployee = await _employeeValidator.ValidateAndGetEntityAsync(
            request.ResponsibleEmployee,
            _employeeRepository,
            "Ответственный сотрудник",
            cancellationToken);

        Counteragent? counteragent = null;
        if (request.Counteragent.HasValue)
        {
            counteragent = await _counteragentValidator.ValidateAndGetEntityAsync(
                request.Counteragent,
                _counteragentRepository,
                "Контрагент",
                cancellationToken);
        }

        var createdProject = new Project
        {
            Name = request.Name,
            Address = request.Address,
            DeadlineDate = request.DeadlineDate.ToUniversalTime(),
            StartDate = request.StartDate.ToUniversalTime(),
            Counteragent = counteragent,
            ResponsibleEmployee = responsibleEmployee,
            ManagerShare = request.ManagerShare,
            ProjectStatus = request.ProjectStatus
        };

        var result = await _projectRepository.AddAsync(createdProject, cancellationToken);
        if (result == RepositoryAddResult.AlreadyExists)
        {
            throw new InvalidOperationException("Проект с такими данными уже существует.");
        }

        _logger.LogInformation("Проект успешно добавлен: {@Project}", createdProject);

        return createdProject;
    }
    
    public async Task<Project> UpdateProjectAsync(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var project = await _projectValidator.ValidateAndGetEntityAsync(
            request.Id,
            _projectRepository,
            "Проект",
            cancellationToken);

        var responsibleEmployee = await _employeeValidator.ValidateAndGetEntityAsync(
            request.ResponsibleEmployee,
            _employeeRepository,
            "Ответственный сотрудник",
            cancellationToken);

        Counteragent? counteragent = null;
        if (request.Counteragent.HasValue)
        {
            counteragent = await _counteragentValidator.ValidateAndGetEntityAsync(
                request.Counteragent,
                _counteragentRepository,
                "Контрагент",
                cancellationToken);
        }
        
        project.Name = request.Name;
        project.Address = request.Address;
        project.DeadlineDate = request.DeadlineDate.ToUniversalTime();
        project.StartDate = request.StartDate.ToUniversalTime();
        project.Counteragent = counteragent;
        project.ResponsibleEmployee = responsibleEmployee;
        project.ManagerShare = request.ManagerShare;
        project.ProjectStatus = request.ProjectStatus;

        var result = await _projectRepository.UpdateAsync(project, cancellationToken);
        if (result == RepositoryUpdateResult.ConcurrencyError)
        {
            throw new ApplicationException(
                "Произошла ошибка конкурентного доступа. Пожалуйста, обновите данные и попробуйте снова.");
        }

        _logger.LogInformation("Проект успешно обновлен: {@Project}", project);

        return project;
    }
    
    public async Task<object?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _projectRepository
            .GetAll()
            .Include(p => p.Counteragent)
            .Include(p => p.ResponsibleEmployee)
            .Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                DeadlineDate = p.DeadlineDate,
                StartDate = p.StartDate,
                Counteragent = p.Counteragent != null ? p.Counteragent.Id : (int?)null,
                ResponsibleEmployee = p.ResponsibleEmployee.Id,
                ProjectStatus = p.ProjectStatus,
                ManagerShare = p.ManagerShare
            })
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }


    public async Task<bool> DeleteProjectAsync(int id, CancellationToken cancellationToken)
    {
        var project = await _projectRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (project == null) return false;

        await _projectRepository.DeleteAsync(project, cancellationToken);
        _logger.LogInformation("Проект с ID {Id} успешно удален", id);
        return true;
    }
    
    public async Task<IEnumerable<object>> GetAllProjectsAsync(CancellationToken cancellationToken)
    {
        var projects = await _projectRepository
            .GetAll()
            .Include(p => p.Counteragent)
            .Include(p => p.ResponsibleEmployee)
            .Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                DeadlineDate = p.DeadlineDate,
                StartDate = p.StartDate,
                Counteragent = p.Counteragent != null ? p.Counteragent.Id : (int?)null,
                ResponsibleEmployee = p.ResponsibleEmployee.Id,
                ProjectStatus = p.ProjectStatus,
                ManagerShare = p.ManagerShare
            })
            .ToListAsync(cancellationToken);

        return projects
            .OrderByDescending(p => p.DeadlineDate)
            .ThenBy(p => p.ProjectStatus)
            .ThenBy(p => p.Name);
    }

    public async Task<object> CalculateProjectWagesAsync(int projectId, CancellationToken ct)
    {
        var project = await _projectRepository.GetAll()
            .FirstOrDefaultAsync(p => p.Id == projectId, ct);

        if (project == null)
        {
            throw new KeyNotFoundException($"Проект с ID {projectId} не найден");
        }

        var projectProducts = await _projectProductRepository.GetAll()
            .Where(pp => pp.Project.Id == projectId)
            .Include(pp => pp.Product)
            .ToListAsync(ct);

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.Project.Id == projectId)
            .ToListAsync(ct);

        var employeeShifts = await _employeeShiftRepository.GetAll()
            .Include(es => es.Employee)
            .Where(es => es.Project.Id == projectId)
            .ToListAsync(ct);

        // Расчет общей суммы наценки
        double totalMarkup = projectProducts.Sum(pp => pp.Markup * pp.Quantity);

        // Вычет доли шефа
        double managerShare = totalMarkup * project.ManagerShare / 100;
        double remainingAmount = totalMarkup - managerShare;

        // Вычет неоплаченных расходов без сотрудника
        double unpaidExpensesWithoutEmployee = expenses
            .Where(e => !e.IsPaidByCompany && e.Employee == null)
            .Sum(e => e.Amount ?? 0);
        remainingAmount -= unpaidExpensesWithoutEmployee;

        // Расчет базовой ставки и общего количества часов
        const double baseHourlyRate = 300;
        double totalHours = employeeShifts.Sum(es => CalculateHoursWorked(es));
        double totalBaseWages = totalHours * baseHourlyRate;

        // Проверка достаточности средств для базовых зарплат
        double adjustedHourlyRate = baseHourlyRate;
        if (remainingAmount < totalBaseWages)
        {
            adjustedHourlyRate = remainingAmount / totalHours;
            remainingAmount = 0;
        }
        else
        {
            remainingAmount -= totalBaseWages;
        }

        // Расчет среднего ИСН и бонусов
        var employeeData = employeeShifts
            .GroupBy(es => es.Employee)
            .Select(g => new
            {
                Employee = g.Key,
                TotalHours = g.Sum(es => CalculateHoursWorked(es)),
                AverageISN = g.Average(es => es.ISN ?? 0)
            })
            .ToList();

        double totalAverageISN = employeeData.Sum(ed => ed.AverageISN);

        var results = employeeData.Select(ed =>
        {
            double baseWage = ed.TotalHours * adjustedHourlyRate;
            double bonus = totalAverageISN > 0 ? (remainingAmount * ed.AverageISN / totalAverageISN) : 0;

            // Расчет компенсации для сотрудника
            double compensation = expenses
                .Where(e => e.IsPaidByCompany && e.Employee?.Id == ed.Employee.Id)
                .Sum(e => e.Amount ?? 0);

            return new
            {
                EmployeeName = ed.Employee.Name,
                TotalHours = ed.TotalHours,
                AverageISN = ed.AverageISN,
                BaseWage = baseWage,
                Bonus = bonus,
                Compensation = compensation,
                TotalWage = baseWage + bonus + compensation
            };
        }).ToList();

        var groupedShifts = employeeShifts
            .GroupBy(es => es.Date.Date)
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                Date = g.Key,
                Shifts = g.Select(es => new
                {
                    EmployeeName = es.Employee.Name,
                    Hours = CalculateHoursWorked(es),
                    ISN = es.ISN ?? 0
                }).ToList()
            })
            .ToList();

        return new
        {
            ProjectId = project.Id,
            EmployeeWages = results,
            DailyShifts = groupedShifts
        };
    }

    private static double CalculateHoursWorked(EmployeeShift shift)
    {
        if (shift.Arrival.HasValue && shift.Departure.HasValue)
        {
            TimeSpan workedTime = shift.Departure.Value - shift.Arrival.Value;
            double hoursWorked = workedTime.TotalHours;

            if (shift.ConsiderTravel && shift.TravelTime.HasValue)
            {
                hoursWorked += shift.TravelTime.Value;
            }

            return hoursWorked;
        }
        return 0;
    }
}