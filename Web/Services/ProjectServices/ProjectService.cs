using Contracts.ProjectEntities;
using Data;

namespace Web.Services.ProjectServices;

/// <summary>
/// Сервис для управления проектами
/// </summary>
public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public int CreateProject(Project project)
    {
        _context.Projects.Add(project);
        _context.SaveChanges();
        return project.Id;
    }

    /// <inheritdoc />
    public void UpdateProject(Project project)
    {
        _context.Projects.Update(project);
        _context.SaveChanges();
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
        var project = _context.Projects.Find(projectId);

        if (project == null)
        {
            throw new ArgumentException("Проект не найден.");
        }

        var totalCost = _context.EmployeeShifts
            .Where(es => es.ProjectId == projectId)
            .Sum(es => es.Wage ?? 0);

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
    public void SuspendProject(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project != null)
        {
            project.IsActive = false;
            _context.SaveChanges();
        }
    }
    
    /// <inheritdoc />
    public void ContinueProject(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project != null)
        {
            project.IsActive = true;
            _context.SaveChanges();
        }
    }
}