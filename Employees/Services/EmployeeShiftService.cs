using Microsoft.Extensions.Logging;
using Contracts.EmployeeEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Employees.Services;

/// <summary>
/// Сервис для управления сменами сотрудников
/// </summary>
public class EmployeeShiftService : IEmployeeShiftService
{
    private readonly IRepository<EmployeeShift> _employeeShiftRepository;
    private readonly ILogger<EmployeeShiftService> _logger;

    public EmployeeShiftService(
        IRepository<EmployeeShift> employeeShiftRepository,
        ILogger<EmployeeShiftService> logger)
    {
        _employeeShiftRepository = employeeShiftRepository;
        _logger = logger;
    }

    public async Task<EmployeeShift> CreateEmployeeShiftAsync(CreateEmployeeShiftRequest request, CancellationToken cancellationToken)
    {
        var createdEmployeeShift = new EmployeeShift
        {
            Project = request.Project,
            Employee = request.Employee,
            Date = request.Date,
            Arrival = request.Arrival,
            Departure = request.Departure,
            HoursWorked = request.HoursWorked,
            TravelTime = request.TravelTime,
            ConsiderTravel = request.ConsiderTravel,
            ISN = request.ISN ?? null
        };
        
        _logger.LogInformation("Смена успешно добавлена: {@Date}", createdEmployeeShift.Date);
        return createdEmployeeShift;
    }
    
    public async Task<EmployeeShift?> UpdateEmployeeShiftAsync(UpdateEmployeeShiftRequest request, CancellationToken cancellationToken)
    {
        var employeeShift = await _employeeShiftRepository.GetAll().FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        if (employeeShift == null) return null;

        employeeShift.Project = request.Project;
        employeeShift.Employee = request.Employee;
        employeeShift.Date = request.Date;
        employeeShift.Arrival = request.Arrival;
        employeeShift.Departure = request.Departure;
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
}