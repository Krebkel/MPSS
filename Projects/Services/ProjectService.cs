using Contracts;
using Microsoft.Extensions.Logging;
using Contracts.ProjectEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Projects.Services;

/// <summary>
/// Сервис для управления логикой проектов
/// </summary>
public class ProjectService : IProjectService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(
        IRepository<Project> projectRepository,
        ILogger<ProjectService> logger)
    {
        _projectRepository = projectRepository;
        _logger = logger;
    }

    public async Task<Project> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var createdProject = new Project
        {
            Name = request.Name,
            Address = request.Address,
            DeadlineDate = request.DeadlineDate,
            StartDate = request.StartDate,
            Counteragent = request.Counteragent,
            ResponsibleEmployee = request.ResponsibleEmployee,
            ManagerShare = request.ManagerShare,
            ProjectStatus = request.ProjectStatus
        };
        
        _logger.LogInformation("Проект успешно добавлен: {@Project}", createdProject);
        return createdProject;
    }
    
    public async Task<Project?> UpdateProjectAsync(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (project == null) return null;

        project.Id = request.Id;
        project.Name = request.Name;
        project.Address = request.Address;
        project.DeadlineDate = request.DeadlineDate;
        project.StartDate = request.StartDate;
        project.Counteragent = request.Counteragent;
        project.ResponsibleEmployee = request.ResponsibleEmployee;
        project.ManagerShare = request.ManagerShare;
        project.ProjectStatus = request.ProjectStatus;

        await _projectRepository.UpdateAsync(project, cancellationToken);

        _logger.LogInformation("Проект успешно обновлен: {@Project}", project);
        return project;
    }
    
    public async Task<Project?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _projectRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteProjectAsync(int id, CancellationToken cancellationToken)
    {
        var project = await _projectRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (project == null) return false;

        await _projectRepository.DeleteAsync(project, cancellationToken);
        _logger.LogInformation("Проект с ID {Id} успешно удален", id);
        return true;
    }
}