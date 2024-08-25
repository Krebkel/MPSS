using Microsoft.Extensions.Logging;
using Contracts.EmployeeEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Employees.Services;

/// <summary>
/// Сервис для работы с сотрудниками
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IRepository<Employee> _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IRepository<Employee> employeeRepository,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }
    
    public async Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var createdEmployee = new Employee
        {
            Name = null,
            Phone = null,
            IsDriver = false,
            Passport = null,
            DateOfBirth = default,
            INN = null,
            AccountNumber = null,
            BIK = null
        };
        
        _logger.LogInformation("Сотрудник успешно добавлен: {@Employee}", createdEmployee);
        return createdEmployee;
    }
    
    public async Task<Employee?> UpdateEmployeeAsync(UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (employee == null) return null;

        employee.Name = request.Name;
        employee.Phone = request.Phone;
        employee.IsDriver = request.IsDriver;
        employee.Passport = request.Passport;
        employee.DateOfBirth = request.DateOfBirth;
        employee.INN = request.INN;
        employee.AccountNumber = request.AccountNumber;
        employee.BIK = request.BIK;

        await _employeeRepository.UpdateAsync(employee, cancellationToken);

        _logger.LogInformation("Сотрудник успешно обновлен: {@Employee}", employee);
        return employee;
    }
    
    public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _employeeRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (employee == null) return false;

        await _employeeRepository.DeleteAsync(employee, cancellationToken);
        _logger.LogInformation("Сотрудник с ID {Id} успешно удален", id);
        return true;
    }
}