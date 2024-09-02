using Microsoft.Extensions.Logging;
using Contracts.ProjectEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Projects.Services;

/// <summary>
/// Сервис для управления логикой проектов
/// </summary>
public class ProjectSuspensionService : IProjectSuspensionService
{
    private readonly IRepository<ProjectSuspension> _projectSuspensionRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IValidator<Project> _projectValidator;
    private readonly IValidator<ProjectSuspension> _projectSuspensionValidator;
    private readonly ILogger<ProjectSuspensionService> _logger;

    public ProjectSuspensionService(
        IRepository<ProjectSuspension> projectSuspensionRepository,
        IRepository<Project> projectRepository,
        ILogger<ProjectSuspensionService> logger,
        IValidator<ProjectSuspension> projectSuspensionValidator,
        IValidator<Project> projectValidator)
    {
        _logger = logger;
        _projectSuspensionRepository = projectSuspensionRepository;
        _projectRepository = projectRepository;
        _projectSuspensionValidator = projectSuspensionValidator;
        _projectValidator = projectValidator;
    }

    public async Task<ProjectSuspension> CreateProjectSuspensionAsync(
        CreateProjectSuspensionRequest request, 
        CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var project = await _projectValidator.ValidateAndGetEntityAsync(
            request.Project, _projectRepository, "Проект", cancellationToken);
        
        var createdProjectSuspension = new ProjectSuspension
        {
            Project = project,
            DateSuspended = request.DateSuspended.ToUniversalTime()
        };
        
        await _projectSuspensionRepository.AddAsync(createdProjectSuspension, cancellationToken);

        _logger.LogInformation("Приостановка проетка успешно добавлена: {@ProjectSuspension}", createdProjectSuspension);
        return createdProjectSuspension;
    }
    
    public async Task<ProjectSuspension> UpdateProjectSuspensionAsync(UpdateProjectSuspensionRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var projectSuspension = await _projectSuspensionValidator.ValidateAndGetEntityAsync(
            request.Id, _projectSuspensionRepository, "Приостановка проекта", cancellationToken);
        
        var project = await _projectValidator.ValidateAndGetEntityAsync(
            request.Project, _projectRepository, "Проект", cancellationToken);

        projectSuspension.Id = request.Id;
        projectSuspension.Project = project;
        projectSuspension.DateSuspended = request.DateSuspended;

        await _projectSuspensionRepository.UpdateAsync(projectSuspension, cancellationToken);

        _logger.LogInformation("Приостановка проекта успешно обновлена: {@ProjectSuspension}", projectSuspension);
        return projectSuspension;
    }
    
    public async Task<object?> GetProjectSuspensionByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _projectSuspensionRepository
            .GetAll()
            .Include(ps => ps.Project)
            .Select(ps => new
            {
                Id = ps.Id,
                Project = ps.Project.Id,
                DateSuspended = ps.DateSuspended
            })
            .FirstOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }


    public async Task<bool> DeleteProjectSuspensionAsync(int id, CancellationToken cancellationToken)
    {
        var projectSuspension = await _projectSuspensionRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (projectSuspension == null) return false;

        await _projectSuspensionRepository.DeleteAsync(projectSuspension, cancellationToken);
        _logger.LogInformation("Приостановка проекта с ID {Id} успешно удалена", id);
        return true;
    }
    
    public async Task<IEnumerable<object>> GetProjectSuspensionsByProjectIdAsync(
        int projectId, CancellationToken cancellationToken)
    {
        return await _projectSuspensionRepository
            .GetAll()
            .Include(ps => ps.Project)
            .Where(ps => ps.Project.Id == projectId)
            .Select(ps => new
            {
                Id = ps.Id,
                Project = ps.Project.Id,
                DateSuspended = ps.DateSuspended
            })
            .ToListAsync(cancellationToken);
    }

}