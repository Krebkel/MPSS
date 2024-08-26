using Contracts;
using Contracts.ProjectEntities;

namespace Projects.Services;

/// <summary>
/// Интерфейс сервиса для работы с проектами.
/// </summary>
public interface IProjectService
{
    Task<Project> CreateProjectAsync(CreateProjectRequest project, CancellationToken cancellationToken);
    
    Task<Project> UpdateProjectAsync(UpdateProjectRequest project, CancellationToken cancellationToken);
    
    Task<Project?> GetProjectByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteProjectAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Project>> GetAllProjectsAsync(CancellationToken cancellationToken);
}

public class UpdateProjectRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required DateTimeOffset DeadlineDate { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public int? Counteragent { get; set; }
    public required int ResponsibleEmployee { get; set; }
    public required ProjectStatus ProjectStatus { get; set; }
    public required float ManagerShare { get; set; }
}

public class CreateProjectRequest
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required DateTimeOffset DeadlineDate { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public int? Counteragent { get; set; }
    public required int ResponsibleEmployee { get; set; }
    public required ProjectStatus ProjectStatus { get; set; }
    public required float ManagerShare { get; set; }
}