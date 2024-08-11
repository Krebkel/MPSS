using Contracts.EmployeeEntities;
using Data;

namespace Employees.Services;

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
    public EmployeeShift GetEmployeeShift(int employeeShiftId)
    {
        return _context.EmployeeShifts.Find(employeeShiftId);
    }
    
    /// <inheritdoc />
    public List<EmployeeShift> GetAllEmployeeShifts(int employeeId)
    {
        return _context.EmployeeShifts
            .Where(es => es.EmployeeId == employeeId)
            .ToList();
    }

    /// <inheritdoc />
    public void UpdateEmployeeShift(EmployeeShift employeeShiftId)
    {
        _context.EmployeeShifts.Update(employeeShiftId);
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