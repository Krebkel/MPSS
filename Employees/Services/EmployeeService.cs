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
        ValidateEmployee(employee);

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
        return _context.Employees.OrderBy(e=>e.Name).ToList();
    }
    
    /// <inheritdoc />
    public void UpdateEmployee(Employee employee)
    {
        ValidateEmployee(employee);
        
        var existingEmployee = _context.Employees.Find(employee.Id);
    
        if (existingEmployee != null)
        {
            existingEmployee.Name = employee.Name ?? existingEmployee.Name;
            existingEmployee.Phone = employee.Phone ?? existingEmployee.Phone;
            existingEmployee.IsDriver = employee.IsDriver;
            existingEmployee.Passport = employee.Passport ?? existingEmployee.Passport;
            existingEmployee.DateOfBirth = employee.DateOfBirth != default
                ? employee.DateOfBirth 
                : existingEmployee.DateOfBirth;
            existingEmployee.INN = employee.INN ?? existingEmployee.INN;
            existingEmployee.AccountNumber = employee.AccountNumber ?? existingEmployee.AccountNumber;
            existingEmployee.BIK = employee.BIK ?? existingEmployee.BIK;

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
    
    private void ValidateEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            throw new ArgumentException("Отсутствует имя сотрудника.");
        }

        if (string.IsNullOrWhiteSpace(employee.Phone))
        {
            throw new ArgumentException("Отсутствует номер телефона сотрудника.");
        }

        if (employee.DateOfBirth == default)
        {
            throw new ArgumentException("Ошибка в дате рождения сотрудника.");
        }
        
        employee.DateOfBirth = employee.DateOfBirth.ToUniversalTime();
    }
}