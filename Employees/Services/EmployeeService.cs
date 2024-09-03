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
    private readonly IValidator<Employee> _employeeValidator;

    public EmployeeService(
        IRepository<Employee> employeeRepository,
        ILogger<EmployeeService> logger,
        IValidator<Employee> employeeValidator)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _employeeValidator = employeeValidator;
    }
    
    public async Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var createdEmployee = new Employee
        {
            Name = request.Name,
            Phone = request.Phone,
            IsDriver = request.IsDriver,
            Passport = request.Passport,
            DateOfBirth = request.DateOfBirth?.ToUniversalTime(),
            INN = request.INN,
            AccountNumber = request.AccountNumber,
            BIK = request.BIK
        };
        
        await _employeeRepository.AddAsync(createdEmployee, cancellationToken);

        _logger.LogInformation("Сотрудник успешно добавлен: {@Employee}", createdEmployee);
        return createdEmployee;
    }
    
    public async Task<Employee> UpdateEmployeeAsync(UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var employee = await _employeeValidator.ValidateAndGetEntityAsync(request.Id,
            _employeeRepository, "Сотрудник", cancellationToken);

        employee.Name = request.Name;
        employee.Phone = request.Phone;
        employee.IsDriver = request.IsDriver;
        employee.Passport = request.Passport;
        employee.DateOfBirth = request.DateOfBirth?.ToUniversalTime();
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
    
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken)
    {
        return await _employeeRepository.GetAll().ToListAsync(cancellationToken);
    }
}