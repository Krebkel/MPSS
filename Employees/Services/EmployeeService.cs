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
    public Employee GetEmployee(int employeeId)
    {
        return _context.Employees.Find(employeeId);
    }
    
    /// <inheritdoc />
    public List<Employee> GetAllEmployees()
    {
        return _context.Employees.ToList();
    }
    
    /// <inheritdoc />
    public void UpdateEmployee(Employee employee)
    {
        var existingEmployee = _context.Employees.Find(employee.Id);
        
        if (existingEmployee != null)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public void DeleteEmployee(int employeeId)
    {
        var employee = _context.Employees.Find(employeeId);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public int AssignShift(EmployeeShift employeeShift)
    {
        _context.EmployeeShifts.Add(employeeShift);
        _context.SaveChanges();
        return employeeShift.Id;
    }
}