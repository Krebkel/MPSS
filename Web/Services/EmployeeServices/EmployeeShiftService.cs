using Contracts.EmployeeEntities;
using Data;

namespace Web.Services.EmployeeServices;

/// <summary>
/// Сервис для управления сменами сотрудников
/// </summary>
public class EmployeeShiftService : IEmployeeShiftService
{
    private readonly AppDbContext _context;

    public EmployeeShiftService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public int CreateEmployeeShift(EmployeeShift employeeShift)
    {
        _context.EmployeeShifts.Add(employeeShift);
        _context.SaveChanges();
        return employeeShift.Id;
    }

    /// <inheritdoc />
    public void UpdateEmployeeShift(EmployeeShift employeeShift)
    {
        _context.EmployeeShifts.Update(employeeShift);
        _context.SaveChanges();
    }

    /// <inheritdoc />
    public void DeleteEmployeeShift(int employeeShiftId)
    {
        var employeeShift = _context.EmployeeShifts.Find(employeeShiftId);
        if (employeeShift != null)
        {
            _context.EmployeeShifts.Remove(employeeShift);
            _context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public float CalculateTotalTime(DateTimeOffset arrival, DateTimeOffset departure)
    {
        return (float)(departure - arrival).TotalHours;
    }
}