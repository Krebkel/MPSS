using Microsoft.Extensions.Logging;
using Contracts.EmployeeEntities;
using Contracts.ProjectEntities;
using Data;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Employees.Services;

/// <summary>
/// Сервис для управления сменами сотрудников
/// </summary>
public class EmployeeShiftService : IEmployeeShiftService
{
    private readonly IRepository<EmployeeShift> _employeeShiftRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly ILogger<EmployeeShiftService> _logger;
    private readonly IValidator<Employee> _employeeValidator;
    private readonly IValidator<Project> _projectValidator;
    private readonly IValidator<EmployeeShift> _employeeShiftValidator;

    public EmployeeShiftService(
        IRepository<EmployeeShift> employeeShiftRepository,
        IRepository<Project> projectRepository,
        IRepository<Employee> employeeRepository,
        ILogger<EmployeeShiftService> logger,
        IValidator<Employee> employeeValidator,
        IValidator<Project> projectValidator,
        IValidator<EmployeeShift> employeeShiftValidator)
    {
        _employeeShiftRepository = employeeShiftRepository;
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
        _logger = logger;
        _employeeValidator = employeeValidator;
        _projectValidator = projectValidator;
        _employeeShiftValidator = employeeShiftValidator;
    }

    public async Task<EmployeeShift> CreateEmployeeShiftAsync(CreateEmployeeShiftRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var project = await _projectValidator.ValidateAndGetEntityAsync(request.Project,
            _projectRepository, "Проект", cancellationToken);
        
        var employee = await _employeeValidator.ValidateAndGetEntityAsync(request.Employee,
            _employeeRepository, "Сотрудник", cancellationToken);

        var createdEmployeeShift = new EmployeeShift
        {
            Project = project,
            Employee = employee,
            Date = request.Date.ToUniversalTime(),
            Arrival = request.Arrival?.ToUniversalTime(),
            Departure = request.Departure?.ToUniversalTime(),
            HoursWorked = request.HoursWorked,
            TravelTime = request.TravelTime,
            ConsiderTravel = request.ConsiderTravel,
            ISN = request.ISN
        };

        await _employeeShiftRepository.AddAsync(createdEmployeeShift, cancellationToken);
        _logger.LogInformation("Смена успешно добавлена: {@Date}", createdEmployeeShift.Date);

        return createdEmployeeShift;
    }
    
    public async Task<EmployeeShift> UpdateEmployeeShiftAsync(UpdateEmployeeShiftRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var employeeShift = await _employeeShiftValidator.ValidateAndGetEntityAsync(request.Id,
            _employeeShiftRepository, "Смена", cancellationToken);
        
        var project = await _projectValidator.ValidateAndGetEntityAsync(request.Project,
            _projectRepository, "Проект", cancellationToken);
        
        var employee = await _employeeValidator.ValidateAndGetEntityAsync(request.Employee,
            _employeeRepository, "Сотрудник", cancellationToken);

        employeeShift.Project = project;
        employeeShift.Employee = employee;
        employeeShift.Date = request.Date.ToUniversalTime();
        employeeShift.Arrival = request.Arrival?.ToUniversalTime();
        employeeShift.Departure = request.Departure?.ToUniversalTime();
        employeeShift.HoursWorked = request.HoursWorked;
        employeeShift.TravelTime = request.TravelTime;
        employeeShift.ConsiderTravel = request.ConsiderTravel;
        employeeShift.ISN = request.ISN;

        await _employeeShiftRepository.UpdateAsync(employeeShift, cancellationToken);
        _logger.LogInformation("Смена успешно обновлена: {@EmployeeShift}", employeeShift);

        return employeeShift;
    }
    
    public async Task<EmployeeShift?> GetEmployeeShiftByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _employeeShiftRepository
            .GetAll()
            .FirstOrDefaultAsync(es => es.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteEmployeeShiftAsync(int id, CancellationToken cancellationToken)
    {
        var employeeShift = await _employeeShiftRepository
            .GetAll()
            .FirstOrDefaultAsync(es => es.Id == id, cancellationToken);
        
        if (employeeShift == null) return false;

        await _employeeShiftRepository.DeleteAsync(employeeShift, cancellationToken);
        _logger.LogInformation("Смена с ID {Id} успешно удален", id);
        return true;
    }
    
    public async Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsByProjectIdAsync(
        int projectId, CancellationToken cancellationToken)
    {
        return await _employeeShiftRepository
            .GetAll()
            .Where(pp => pp.Project.Id == projectId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsByEmployeeIdAsync(
        int employeeId, CancellationToken cancellationToken)
    {
        return await _employeeShiftRepository
            .GetAll()
            .Where(es => es.Employee.Id == employeeId)
            .ToListAsync(cancellationToken);
    }
}