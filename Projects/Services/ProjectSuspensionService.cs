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
    private readonly ILogger<ProjectSuspensionService> _logger;

    public ProjectSuspensionService(
        IRepository<ProjectSuspension> projectSuspensionRepository,
        ILogger<ProjectSuspensionService> logger)
    {
        _projectSuspensionRepository = projectSuspensionRepository;
        _logger = logger;
    }

    public async Task<ProjectSuspension> CreateProjectSuspensionAsync(
        CreateProjectSuspensionRequest request, 
        CancellationToken cancellationToken)
    {
        var createdProjectSuspension = new ProjectSuspension
        {
            Project = request.Project,
            DateSuspended = request.DateSuspended
        };
        
        _logger.LogInformation("Приостановка проетка успешно добавлена: {@ProjectSuspension}", createdProjectSuspension);
        return createdProjectSuspension;
    }
    
    public async Task<ProjectSuspension?> UpdateProjectSuspensionAsync(UpdateProjectSuspensionRequest request, CancellationToken cancellationToken)
    {
        var projectSuspension = await _projectSuspensionRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (projectSuspension == null) return null;

        projectSuspension.Id = request.Id;
        projectSuspension.Project = request.Project;
        projectSuspension.DateSuspended = request.DateSuspended;

        await _projectSuspensionRepository.UpdateAsync(projectSuspension, cancellationToken);

        _logger.LogInformation("Приостановка проекта успешно обновлена: {@ProjectSuspension}", projectSuspension);
        return projectSuspension;
    }
    
    public async Task<ProjectSuspension?> GetProjectSuspensionByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _projectSuspensionRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
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
}