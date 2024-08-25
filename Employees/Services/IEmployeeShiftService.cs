using Contracts.EmployeeEntities;
using Contracts.ProjectEntities;

namespace Employees.Services;

/// <summary>
/// Сервис для управления сменами сотрудников
/// </summary>
public interface IEmployeeShiftService
{
    Task<EmployeeShift> CreateEmployeeShiftAsync(CreateEmployeeShiftRequest employeeShift, CancellationToken cancellationToken);
    
    Task<EmployeeShift> UpdateEmployeeShiftAsync(UpdateEmployeeShiftRequest employeeShift, CancellationToken cancellationToken);
    
    Task<EmployeeShift?> GetEmployeeShiftByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteEmployeeShiftAsync(int id, CancellationToken cancellationToken);
}

public class UpdateEmployeeShiftRequest
{
    public required int Id { get; set; }
    public required Project Project { get; set; }
    public required Employee Employee { get; set; }
    public required DateTimeOffset Date { get; set; }
    public DateTimeOffset? Arrival { get; set; }
    public DateTimeOffset? Departure { get; set; }
    public float? HoursWorked { get; set; }
    public float? TravelTime { get; set; }
    public bool ConsiderTravel { get; set; }
    public int? ISN { get; set; }
}

public class CreateEmployeeShiftRequest
{
    public required Project Project { get; set; }
    public required Employee Employee { get; set; }
    public required DateTimeOffset Date { get; set; }
    public DateTimeOffset? Arrival { get; set; }
    public DateTimeOffset? Departure { get; set; }
    public float? HoursWorked { get; set; }
    public float? TravelTime { get; set; }
    public bool ConsiderTravel { get; set; }
    public int? ISN { get; set; }
}