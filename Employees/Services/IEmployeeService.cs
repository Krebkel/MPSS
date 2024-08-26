using Contracts.EmployeeEntities;

namespace Employees.Services;

/// <summary>
/// Интерфейс сервиса для работы с сотрудниками.
/// </summary>
public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest employee, CancellationToken cancellationToken);
    
    Task<Employee> UpdateEmployeeAsync(UpdateEmployeeRequest employee, CancellationToken cancellationToken);
    
    Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken);
}

public class UpdateEmployeeRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public bool IsDriver { get; set; }
    public string? Passport { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public string? INN { get; set; }
    public string? AccountNumber { get; set; }
    public string? BIK { get; set; }
}

public class CreateEmployeeRequest
{
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public bool IsDriver { get; set; }
    public string? Passport { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public string? INN { get; set; }
    public string? AccountNumber { get; set; }
    public string? BIK { get; set; }
}