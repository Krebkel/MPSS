using Contracts.EmployeeEntities;
using Contracts.ProjectEntities;
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
    public void UpdateEmployeeShift(EmployeeShift employeeShift)
    {
        var existingShift = _context.EmployeeShifts.Find(employeeShift);
        
        if (existingShift != null)
        {
            _context.EmployeeShifts.Update(employeeShift);
            _context.SaveChanges();
        }
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
    public float CalculateTotalTime(DateTimeOffset? arrival, DateTimeOffset? departure)
    {
        if (arrival.HasValue && departure.HasValue)
        {
            return (float)(departure.Value - arrival.Value).TotalHours;
        }
        return 0;
    }
    
    /// <inheritdoc />
    public double CalculateTotalWage(int employeeId, int projectId)
    {
        var employeeShifts = _context.EmployeeShifts
            .Where(es => es.EmployeeId == employeeId && es.ProjectId == projectId)
            .ToList();

        if (employeeShifts.Count == 0)
        {
            return 0;
        }

        var project = _context.Projects.Find(projectId);
        if (project == null)
        {
            throw new ArgumentException("Project not found");
        }

        double totalBaseWage = employeeShifts.Sum(es => CalculateTotalTime(es.Arrival, es.Departure) * 300);

        double totalMarkup = _context.ProjectProducts
            .Where(pp => pp.ProjectId == projectId)
            .Sum(pp => pp.Markup * pp.Quantity);

        double expensesNotPaidByCompany = _context.Expenses
            .Where(e => e.ProjectId == projectId && !e.IsPaidByCompany)
            .Sum(e => e.Amount ?? 0);

        double managerShare = totalMarkup * (project.ManagerShare / 100);
        double distributableAmount = totalMarkup - expensesNotPaidByCompany - managerShare;

        var totalISN = _context.EmployeeShifts
            .Where(es => es.ProjectId == projectId)
            .Sum(es => es.ISN);

        double employeeISN = employeeShifts.Sum(es => es.ISN);
        double totalBonus = (distributableAmount * employeeISN) / totalISN;

        return totalBaseWage + totalBonus;
    }
    
    /// <inheritdoc />
    public double CalculateTotalWageForDoneProjects(int employeeId)
    {
        var doneProjects = _context.Projects
            .Where(p => p.ProjectStatus == ProjectStatus.Done)
            .Select(p => p.Id)
            .ToList();

        double totalWage = 0;

        foreach (var projectId in doneProjects)
        {
            totalWage += CalculateTotalWage(employeeId, projectId);
        }

        return totalWage;
    }
}