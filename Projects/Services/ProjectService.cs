using Contracts.ProjectEntities;
using Data;
using Employees.Services;

namespace Projects.Services;

/// <summary>
/// Сервис для управления проектами
/// </summary>
public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly IEmployeeShiftService _employeeShiftService;

    public ProjectService(AppDbContext context, IEmployeeShiftService employeeShiftService)
    {
        _context = context;
        _employeeShiftService = employeeShiftService;
    }

    /// <inheritdoc />
    public int CreateProject(Project project)
    {
        ValidateProject(project);

        _context.Projects.Add(project);
        _context.SaveChanges();
        return project.Id;
    }
    
    /// <inheritdoc />
    public Project GetProject(int projectId)
    {
        return _context.Projects.Find(projectId);
    }
    
    /// <inheritdoc />
    public List<Project> GetAllProjects()
    {
        return _context.Projects.ToList();
    }

    /// <inheritdoc />
    public void UpdateProject(Project project)
    {
        ValidateProject(project);

        var existingProject = _context.Projects.Find(project.Id);
        if (existingProject != null)
        {
            existingProject.Name = project.Name ?? existingProject.Name;
            existingProject.Address = project.Address ?? existingProject.Address;
            existingProject.DeadlineDate = project.DeadlineDate != default
                ? project.DeadlineDate 
                : existingProject.DeadlineDate;
            existingProject.StartDate = project.StartDate != default
                ? project.StartDate 
                : existingProject.StartDate;
            existingProject.DateSuspended = project.DateSuspended ?? existingProject.DateSuspended;
            existingProject.CounteragentId = project.CounteragentId ?? existingProject.CounteragentId;
            existingProject.ResponsibleEmployeeId = project.ResponsibleEmployeeId != default
                ? project.ResponsibleEmployeeId 
                : existingProject.ResponsibleEmployeeId;
            existingProject.ProjectStatus = project.ProjectStatus != default
                ? project.ProjectStatus 
                : existingProject.ProjectStatus;
            existingProject.ManagerShare = project.ManagerShare != default
                ? project.ManagerShare 
                : existingProject.ManagerShare;

            _context.SaveChanges();
        }
    }


    /// <inheritdoc />
    public void DeleteProject(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project != null)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public double CalculateTotalCost(int projectId)
    {
        var projectProducts = _context.ProjectProducts
            .Where(pp => pp.ProjectId == projectId)
            .ToList();

        double totalCost = 0;

        foreach (var projectProduct in projectProducts)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == projectProduct.ProductId);

            if (product != null)
            {
                totalCost += projectProduct.Quantity * (product.Cost + projectProduct.Markup);
            }
        }

        return totalCost;
    }

    /// <inheritdoc />
    public double CalculateAverageProductivity(int projectId)
    {
        var project = _context.Projects.Find(projectId);

        if (project == null)
        {
            throw new ArgumentException("Проект не найден.");
        }

        var shifts = _context.EmployeeShifts
            .Where(es => es.ProjectId == projectId)
            .ToList();

        if (shifts.Count == 0)
        {
            return 0;
        }

        var totalProductivity = shifts.Sum(es => es.HoursWorked ?? 0);
        var shiftCount = shifts.Count;

        return shiftCount > 0 ? totalProductivity / shiftCount : 0;
    }

    /// <inheritdoc />
    public void ChangeProjectStatus(int projectId, ProjectStatus projectStatus)
    {
        var project = _context.Projects.Find(projectId);
        if (project != null)
        {
            project.ProjectStatus = projectStatus;
            _context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public void DistributeProjectBonus(int projectId, double managerShare)
    {
        var project = _context.Projects.Find(projectId);
        if (project == null)
        {
            throw new ArgumentException("Проект не найден.");
        }

        var shifts = _context.EmployeeShifts
            .Where(es => es.ProjectId == projectId)
            .ToList();

        if (shifts.Count == 0)
        {
            return;
        }

        double totalMarkup = _context.ProjectProducts
            .Where(pp => pp.ProjectId == projectId)
            .Sum(pp => pp.Markup * pp.Quantity);

        double expensesNotPaidByCompany = _context.Expenses
            .Where(e => e.ProjectId == projectId && !e.IsPaidByCompany)
            .Sum(e => e.Amount ?? 0);

        double totalISN = shifts.Sum(es => es.ISN);

        double distributableAmount = totalMarkup - managerShare - expensesNotPaidByCompany;

        foreach (var shift in shifts)
        {
            double employeeBonus = (shift.ISN / totalISN) * distributableAmount;
            double totalWage = _employeeShiftService.CalculateTotalWage(shift.EmployeeId, projectId) + employeeBonus;
        }

        _context.SaveChanges();
    }

    /// <inheritdoc />
    public double CalculateTotalWageForDoneProjects(int employeeId)
    {
        return _employeeShiftService.CalculateTotalWageForDoneProjects(employeeId);
    }

    /// <inheritdoc />
    private void ValidateProject(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.Name))
        {
            throw new ArgumentException("Отсутствует название проекта.");
        }

        if (project.CounteragentId <= 0)
        {
            throw new ArgumentException("Не выбран контрагент.");
        }

        if (string.IsNullOrWhiteSpace(project.Address))
        {
            throw new ArgumentException("Отсутствует адрес проекта.");
        }

        if (project.DeadlineDate == default)
            throw new ArgumentException("Отсутствует дедлайн проекта.");
        
        project.StartDate = project.StartDate.ToUniversalTime();
        project.DeadlineDate = project.DeadlineDate.ToUniversalTime();
    }
}