using Contracts.EmployeeEntities;

namespace Employees.Services;

/// <summary>
/// Сервис для управления сменами сотрудников
/// </summary>
public interface IEmployeeShiftService
{
    Task<EmployeeShift> CreateEmployeeShiftAsync(CreateEmployeeShiftRequest employeeShift, CancellationToken cancellationToken);

    Task<EmployeeShift> UpdateEmployeeShiftAsync(UpdateEmployeeShiftRequest employeeShift, CancellationToken cancellationToken);
    
    Task<object?> GetEmployeeShiftByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteEmployeeShiftAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<object>> GetEmployeeShiftsByProjectIdAsync(int projectId, CancellationToken cancellationToken);
    
    Task<IEnumerable<object>> GetEmployeeShiftsByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
}

public class UpdateEmployeeShiftRequest
{
    public required int Id { get; set; }
    public required int Project { get; set; }
    public required int Employee { get; set; }
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
    public required int Project { get; set; }
    public required int Employee { get; set; }
    public required DateTimeOffset Date { get; set; }
    public DateTimeOffset? Arrival { get; set; }
    public DateTimeOffset? Departure { get; set; }
    public float? HoursWorked { get; set; }
    public float? TravelTime { get; set; }
    public bool ConsiderTravel { get; set; }
    public int? ISN { get; set; }
}