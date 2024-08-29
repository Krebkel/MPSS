using Contracts.EmployeeEntities;
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
    private readonly IRepository<Counteragent> _counteragentRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly ILogger<ProjectService> _logger;
    private readonly IValidator<Employee> _employeeValidator;
    private readonly IValidator<Counteragent> _counteragentValidator;
    private readonly IValidator<Project> _projectValidator;
    
    public ProjectService(
        ILogger<ProjectService> logger,
        IRepository<Project> projectRepository,
        IRepository<Counteragent> counteragentRepository,
        IRepository<Employee> employeeRepository,
        IValidator<Project> projectValidator,
        IValidator<Counteragent> counteragentValidator,
        IValidator<Employee> employeeValidator)
    {
        _logger = logger;
        _projectRepository = projectRepository;
        _counteragentRepository = counteragentRepository;
        _employeeRepository = employeeRepository;
        _projectValidator = projectValidator;
        _counteragentValidator = counteragentValidator;
        _employeeValidator = employeeValidator;
    }

    public async Task<Project> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var responsibleEmployee = await _employeeValidator.ValidateAndGetEntityAsync(
            request.ResponsibleEmployee,
            _employeeRepository,
            "Ответственный сотрудник",
            cancellationToken);

        Counteragent? counteragent = null;
        if (request.Counteragent.HasValue)
        {
            counteragent = await _counteragentValidator.ValidateAndGetEntityAsync(
                request.Counteragent,
                _counteragentRepository,
                "Контрагент",
                cancellationToken);
        }

        var createdProject = new Project
        {
            Name = request.Name,
            Address = request.Address,
            DeadlineDate = request.DeadlineDate.ToUniversalTime(),
            StartDate = request.StartDate.ToUniversalTime(),
            Counteragent = counteragent,
            ResponsibleEmployee = responsibleEmployee,
            ManagerShare = request.ManagerShare,
            ProjectStatus = request.ProjectStatus
        };

        var result = await _projectRepository.AddAsync(createdProject, cancellationToken);
        if (result == RepositoryAddResult.AlreadyExists)
        {
            throw new InvalidOperationException("Проект с такими данными уже существует.");
        }

        _logger.LogInformation("Проект успешно добавлен: {@Project}", createdProject);

        return createdProject;
    }
    
    public async Task<Project> UpdateProjectAsync(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var project = await _projectValidator.ValidateAndGetEntityAsync(
            request.Id,
            _projectRepository,
            "Проект",
            cancellationToken);

        var responsibleEmployee = await _employeeValidator.ValidateAndGetEntityAsync(
            request.ResponsibleEmployee,
            _employeeRepository,
            "Ответственный сотрудник",
            cancellationToken);

        Counteragent? counteragent = null;
        if (request.Counteragent.HasValue)
        {
            counteragent = await _counteragentValidator.ValidateAndGetEntityAsync(
                request.Counteragent,
                _counteragentRepository,
                "Контрагент",
                cancellationToken);
        }
        
        project.Name = request.Name;
        project.Address = request.Address;
        project.DeadlineDate = request.DeadlineDate.ToUniversalTime();
        project.StartDate = request.StartDate.ToUniversalTime();
        project.Counteragent = counteragent;
        project.ResponsibleEmployee = responsibleEmployee;
        project.ManagerShare = request.ManagerShare;
        project.ProjectStatus = request.ProjectStatus;

        var result = await _projectRepository.UpdateAsync(project, cancellationToken);
        if (result == RepositoryUpdateResult.ConcurrencyError)
        {
            throw new ApplicationException(
                "Произошла ошибка конкурентного доступа. Пожалуйста, обновите данные и попробуйте снова.");
        }

        _logger.LogInformation("Проект успешно обновлен: {@Project}", project);

        return project;
    }
    
    public async Task<object?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _projectRepository
            .GetAll()
            .Include(p => p.Counteragent)
            .Include(p => p.ResponsibleEmployee)
            .Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                DeadlineDate = p.DeadlineDate,
                StartDate = p.StartDate,
                Counteragent = p.Counteragent != null ? p.Counteragent.Id : (int?)null,
                ResponsibleEmployee = p.ResponsibleEmployee.Id,
                ProjectStatus = p.ProjectStatus,
                ManagerShare = p.ManagerShare
            })
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
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
    
    public async Task<IEnumerable<object>> GetAllProjectsAsync(CancellationToken cancellationToken)
    {
        return await _projectRepository
            .GetAll()
            .Include(p => p.Counteragent)
            .Include(p => p.ResponsibleEmployee)
            .Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                DeadlineDate = p.DeadlineDate,
                StartDate = p.StartDate,
                Counteragent = p.Counteragent != null ? p.Counteragent.Id : (int?)null,
                ResponsibleEmployee = p.ResponsibleEmployee.Id,
                ProjectStatus = p.ProjectStatus,
                ManagerShare = p.ManagerShare
            })
            .ToListAsync(cancellationToken);
    }

}