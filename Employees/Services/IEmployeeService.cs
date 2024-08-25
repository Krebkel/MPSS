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
}

public class UpdateEmployeeRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public bool IsDriver { get; set; }
    public ulong? Passport { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public ulong? INN { get; set; }
    public ulong? AccountNumber { get; set; }
    public ulong? BIK { get; set; }
}

public class CreateEmployeeRequest
{
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public bool IsDriver { get; set; }
    public ulong? Passport { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public ulong? INN { get; set; }
    public ulong? AccountNumber { get; set; }
    public ulong? BIK { get; set; }
}