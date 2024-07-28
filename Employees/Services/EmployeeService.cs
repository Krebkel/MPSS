using Contracts.EmployeeEntities;
using Data;

namespace Employees.Services;

/// <summary>
/// Сервис для работы с сотрудниками
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public int CreateEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
        return employee.Id;
    }

    /// <inheritdoc />
    public void UpdateEmployee(Employee employee)
    {
        _context.Employees.Update(employee);
        _context.SaveChanges();
    }

    /// <inheritdoc />
    public int AssignShift(EmployeeShift employeeShift)
    {
        _context.EmployeeShifts.Add(employeeShift);
        _context.SaveChanges();
        return employeeShift.Id;
    }

    /// <inheritdoc />
    public double CalculateWage(int employeeId, int projectId)
    {
        var shifts = _context.EmployeeShifts
            .Where(es => es.EmployeeId == employeeId && es.ProjectId == projectId)
            .ToList();

        double totalWage = 0;
        foreach (var shift in shifts)
        {
            if (shift.Wage.HasValue)
            {
                totalWage += shift.Wage.Value;
            }
        }

        return totalWage;
    }

    /// <inheritdoc />
    public double CalculateShiftWage(int employeeId, int employeeShiftId)
    {
        var shift = _context.EmployeeShifts
            .FirstOrDefault(es => es.EmployeeId == employeeId && es.Id == employeeShiftId);

        if (shift == null)
        {
            throw new ArgumentException("Смена не найдена.");
        }

        return shift.Wage ?? 0;
    }
}